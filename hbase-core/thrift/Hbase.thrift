/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

# Thrift Tutorial
# Mark Slee (mcslee@facebook.com)
#
# This file aims to teach you how to use Thrift, in a .thrift file. Neato. The
# first thing to notice is that .thrift files support standard shell comments.
# This lets you make your thrift file executable and include your Thrift build
# step on the top line. And you can place comments like this anywhere you like.
#
# Before running this file, you will need to have installed the thrift compiler
# into /usr/local/bin.

/**
 * The first thing to know about are types. The available types in Thrift are:
 *
 *  bool        Boolean, one byte
 *  i8 (byte)   Signed 8-bit integer
 *  i16         Signed 16-bit integer
 *  i32         Signed 32-bit integer
 *  i64         Signed 64-bit integer
 *  double      64-bit floating point value
 *  string      String
 *  binary      Blob (byte array)
 *  map<t1,t2>  Map from one type to another
 *  list<t1>    Ordered list of one type
 *  set<t1>     Set of unique elements of one type
 *
 * Did you also notice that Thrift supports C style comments?
 */

// Just in case you were wondering... yes. We support simple C comments too.

/**
 * Thrift files can reference other Thrift files to include common struct
 * and service definitions. These are found using the current path, or by
 * searching relative to any paths specified with the -I compiler flag.
 *
 * Included objects are accessed using the name of the .thrift file as a
 * prefix. i.e. shared.SharedObject
 */

/**
 * Thrift files can namespace, package, or prefix their output in various
 * target languages.
 */

namespace java org.apache.hadoop.hbase.thrift.generated
namespace cpp  apache.hadoop.hbase.thrift
namespace rb Apache.Hadoop.Hbase.Thrift
namespace py hbase
namespace perl Hbase
namespace php Hbase
namespace netcore Hbase.Thrift
//
// Types
//

// NOTE: all variables with the Text type are assumed to be correctly
// formatted UTF-8 strings.  This is a programming language and locale
// dependent property that the client application is repsonsible for
// maintaining.  If strings with an invalid encoding are sent, an
// IOError will be thrown.

typedef binary Text
typedef binary Bytes
typedef i32    ScannerID

/**
 * TCell - Used to transport a cell value (byte[]) and the timestamp it was 
 * stored with together as a result for get and getRow methods. This promotes
 * the timestamp of a cell to a first-class value, making it easy to take 
 * note of temporal data. Cell is used all the way from HStore up to HTable.
 */
struct TCell{
  1:Bytes value,
  2:i64 timestamp
}

/**
 * An HColumnDescriptor contains information about a column family
 * such as the number of versions, compression settings, etc. It is
 * used as input when creating a table or adding a column.
 */
struct ColumnDescriptor {
  1:Text name,
  2:i32 maxVersions = 3,
  3:string compression = "NONE",
  4:bool inMemory = 0,
  5:string bloomFilterType = "NONE",
  6:i32 bloomFilterVectorSize = 0,
  7:i32 bloomFilterNbHashes = 0,
  8:bool blockCacheEnabled = 0,
  9:i32 timeToLive = 0x7fffffff
}

/**
 * A TRegionInfo contains information about an HTable region.
 */
struct TRegionInfo {
  1:Text startKey,
  2:Text endKey,
  3:i64 id,
  4:Text name,
  5:i8 version,
  6:Text serverName,
  7:i32 port
}

/**
 * A Mutation object is used to either update or delete a column-value.
 */
struct Mutation {
  1:bool isDelete = 0,
  2:Text column,
  3:Text value,
  4:bool writeToWAL = 1
}


/**
 * A BatchMutation object is used to apply a number of Mutations to a single row.
 */
struct BatchMutation {
  1:Text row,
  2:list<Mutation> mutations
}

/**
 * For increments that are not incrementColumnValue
 * equivalents.
 */
struct TIncrement {
  1:Text table,
  2:Text row,
  3:Text column,
  4:i64  ammount
}

/**
 * Holds column name and the cell.
 */
struct TColumn {
  1:Text columnName,
  2:TCell cell
 }

/**
 * Holds row name and then a map of columns to cells. 
 */
struct TRowResult {
  1:Text row,
  2:optional map<Text, TCell> columns,
  3:optional list<TColumn> sortedColumns
}

/**
 * A Scan object is used to specify scanner parameters when opening a scanner.
 */
struct TScan {
  1:optional Text startRow,
  2:optional Text stopRow,
  3:optional i64 timestamp,
  4:optional list<Text> columns,
  5:optional i32 caching,
  6:optional Text filterString,
  7:optional i32 batchSize,
  8:optional bool sortColumns,
  9:optional bool reversed,
  10:optional bool cacheBlocks
}

/**
 * An Append object is used to specify the parameters for performing the append operation.
 */
struct TAppend {
  1:Text table,
  2:Text row,
  3:list<Text> columns,
  4:list<Text> values
}

//
// Exceptions
//
/**
 * An IOError exception signals that an error occurred communicating
 * to the Hbase master or an Hbase region server.  Also used to return
 * more general Hbase error conditions.
 */
exception IOError {
  1:string message
}

/**
 * An IllegalArgument exception indicates an illegal or invalid
 * argument was passed into a procedure.
 */
exception IllegalArgument {
  1:string message
}

/**
 * An AlreadyExists exceptions signals that a table with the specified
 * name already exists
 */
exception AlreadyExists {
  1:string message
}

//
// Service 
//

service Hbase {
  /**
   * Brings a table on-line (enables it)
   */
  void enableTable(
    /** name of the table */
    1:Bytes tableName
  ) throws (1:IOError io)
    
  /**
   * Disables a table (takes it off-line) If it is being served, the master
   * will tell the servers to stop serving it.
   */
  void disableTable(
    /** name of the table */
    1:Bytes tableName
  ) throws (1:IOError io)

  /**
   * @return true if table is on-line
   */
  bool isTableEnabled(
    /** name of the table to check */
    1:Bytes tableName
  ) throws (1:IOError io)
    
  void compact(1:Bytes tableNameOrRegionName)
    throws (1:IOError io)
  
  void majorCompact(1:Bytes tableNameOrRegionName)
    throws (1:IOError io)
    
  /**
   * List all the userspace tables.
   *
   * @return returns a list of names
   */
  list<Text> getTableNames()
    throws (1:IOError io)

  /**
   * List all the column families assoicated with a table.
   *
   * @return list of column family descriptors
   */
  map<Text,ColumnDescriptor> getColumnDescriptors (
    /** table name */
    1:Text tableName
  ) throws (1:IOError io)

  /**
   * List the regions associated with a table.
   *
   * @return list of region descriptors
   */
  list<TRegionInfo> getTableRegions(
    /** table name */
    1:Text tableName)
    throws (1:IOError io)

  /**
   * Create a table with the specified column families.  The name
   * field for each ColumnDescriptor must be set and must end in a
   * colon (:). All other fields are optional and will get default
   * values if not explicitly specified.
   *
   * @throws IllegalArgument if an input parameter is invalid
   *
   * @throws AlreadyExists if the table name already exists
   */
  void createTable(
    /** name of table to create */
    1:Text tableName,

    /** list of column family descriptors */
    2:list<ColumnDescriptor> columnFamilies
  ) throws (1:IOError io, 2:IllegalArgument ia, 3:AlreadyExists exist)

  /**
   * Deletes a table
   *
   * @throws IOError if table doesn't exist on server or there was some other
   * problem
   */
  void deleteTable(
    /** name of table to delete */
    1:Text tableName
  ) throws (1:IOError io)

  /** 
   * Get a single TCell for the specified table, row, and column at the
   * latest timestamp. Returns an empty list if no such value exists.
   *
   * @return value for specified row/column
   */
  list<TCell> get(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** column name */
    3:Text column,

    /** Get attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get the specified number of versions for the specified table,
   * row, and column.
   *
   * @return list of cells for specified row/column
   */
  list<TCell> getVer(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** column name */
    3:Text column,

    /** number of versions to retrieve */
    4:i32 numVersions,

    /** Get attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get the specified number of versions for the specified table,
   * row, and column.  Only versions less than or equal to the specified
   * timestamp will be returned.
   *
   * @return list of cells for specified row/column
   */
  list<TCell> getVerTs(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** column name */
    3:Text column,

    /** timestamp */
    4:i64 timestamp,

    /** number of versions to retrieve */
    5:i32 numVersions,

    /** Get attributes */
    6:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get all the data for the specified table and row at the latest
   * timestamp. Returns an empty list if the row does not exist.
   * 
   * @return TRowResult containing the row and map of columns to TCells
   */
  list<TRowResult> getRow(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** Get attributes */
    3:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get the specified columns for the specified table and row at the latest
   * timestamp. Returns an empty list if the row does not exist.
   * 
   * @return TRowResult containing the row and map of columns to TCells
   */
  list<TRowResult> getRowWithColumns(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** List of columns to return, null for all columns */
    3:list<Text> columns,

    /** Get attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get all the data for the specified table and row at the specified
   * timestamp. Returns an empty list if the row does not exist.
   * 
   * @return TRowResult containing the row and map of columns to TCells
   */
  list<TRowResult> getRowTs(
    /** name of the table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** timestamp */
    3:i64 timestamp,

    /** Get attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)
    
  /** 
   * Get the specified columns for the specified table and row at the specified
   * timestamp. Returns an empty list if the row does not exist.
   * 
   * @return TRowResult containing the row and map of columns to TCells
   */
  list<TRowResult> getRowWithColumnsTs(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** List of columns to return, null for all columns */
    3:list<Text> columns,
    4:i64 timestamp,

    /** Get attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Get all the data for the specified table and rows at the latest
   * timestamp. Returns an empty list if no rows exist.
   *
   * @return TRowResult containing the rows and map of columns to TCells
   */
  list<TRowResult> getRows(
    /** name of table */
    1:Text tableName,

    /** row keys */
    2:list<Text> rows

    /** Get attributes */
    3:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Get the specified columns for the specified table and rows at the latest
   * timestamp. Returns an empty list if no rows exist.
   *
   * @return TRowResult containing the rows and map of columns to TCells
   */
  list<TRowResult> getRowsWithColumns(
    /** name of table */
    1:Text tableName,

    /** row keys */
    2:list<Text> rows,

    /** List of columns to return, null for all columns */
    3:list<Text> columns,

    /** Get attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Get all the data for the specified table and rows at the specified
   * timestamp. Returns an empty list if no rows exist.
   *
   * @return TRowResult containing the rows and map of columns to TCells
   */
  list<TRowResult> getRowsTs(
    /** name of the table */
    1:Text tableName,

    /** row keys */
    2:list<Text> rows

    /** timestamp */
    3:i64 timestamp,

    /** Get attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Get the specified columns for the specified table and rows at the specified
   * timestamp. Returns an empty list if no rows exist.
   *
   * @return TRowResult containing the rows and map of columns to TCells
   */
  list<TRowResult> getRowsWithColumnsTs(
    /** name of table */
    1:Text tableName,

    /** row keys */
    2:list<Text> rows

    /** List of columns to return, null for all columns */
    3:list<Text> columns,
    4:i64 timestamp,

    /** Get attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Apply a series of mutations (updates/deletes) to a row in a
   * single transaction.  If an exception is thrown, then the
   * transaction is aborted.  Default current timestamp is used, and
   * all entries will have an identical timestamp.
   */
  void mutateRow(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** list of mutation commands */
    3:list<Mutation> mutations,

    /** Mutation attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /** 
   * Apply a series of mutations (updates/deletes) to a row in a
   * single transaction.  If an exception is thrown, then the
   * transaction is aborted.  The specified timestamp is used, and
   * all entries will have an identical timestamp.
   */
  void mutateRowTs(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** list of mutation commands */
    3:list<Mutation> mutations,

    /** timestamp */
    4:i64 timestamp,

    /** Mutation attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /** 
   * Apply a series of batches (each a series of mutations on a single row)
   * in a single transaction.  If an exception is thrown, then the
   * transaction is aborted.  Default current timestamp is used, and
   * all entries will have an identical timestamp.
   */
  void mutateRows(
    /** name of table */
    1:Text tableName,

    /** list of row batches */
    2:list<BatchMutation> rowBatches,

    /** Mutation attributes */
    3:map<Text, Text> attributes
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /** 
   * Apply a series of batches (each a series of mutations on a single row)
   * in a single transaction.  If an exception is thrown, then the
   * transaction is aborted.  The specified timestamp is used, and
   * all entries will have an identical timestamp.
   */
  void mutateRowsTs(
    /** name of table */
    1:Text tableName,

    /** list of row batches */
    2:list<BatchMutation> rowBatches,

    /** timestamp */
    3:i64 timestamp,

    /** Mutation attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /**
   * Atomically increment the column value specified.  Returns the next value post increment.
   */
  i64 atomicIncrement(
    /** name of table */
    1:Text tableName,

    /** row to increment */
    2:Text row,

    /** name of column */
    3:Text column,

    /** amount to increment by */
    4:i64 value
  ) throws (1:IOError io, 2:IllegalArgument ia)
    
  /** 
   * Delete all cells that match the passed row and column.
   */
  void deleteAll(
    /** name of table */
    1:Text tableName,

    /** Row to update */
    2:Text row,

    /** name of column whose value is to be deleted */
    3:Text column,

    /** Delete attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Delete all cells that match the passed row and column and whose
   * timestamp is equal-to or older than the passed timestamp.
   */
  void deleteAllTs(
    /** name of table */
    1:Text tableName,

    /** Row to update */
    2:Text row,

    /** name of column whose value is to be deleted */
    3:Text column,

    /** timestamp */
    4:i64 timestamp,

    /** Delete attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Completely delete the row's cells.
   */
  void deleteAllRow(
    /** name of table */
    1:Text tableName,

    /** key of the row to be completely deleted. */
    2:Text row,

    /** Delete attributes */
    3:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Increment a cell by the ammount.
   * Increments can be applied async if hbase.regionserver.thrift.coalesceIncrement is set to true.
   * False is the default.  Turn to true if you need the extra performance and can accept some
   * data loss if a thrift server dies with increments still in the queue.
   */
  void increment(
    /** The single increment to apply */
    1:TIncrement increment
  ) throws (1:IOError io)


  void incrementRows(
    /** The list of increments */
    1:list<TIncrement> increments
  ) throws (1:IOError io)

  /**
   * Completely delete the row's cells marked with a timestamp
   * equal-to or older than the passed timestamp.
   */
  void deleteAllRowTs(
    /** name of table */
    1:Text tableName,

    /** key of the row to be completely deleted. */
    2:Text row,

    /** timestamp */
    3:i64 timestamp,

    /** Delete attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Get a scanner on the current table, using the Scan instance
   * for the scan parameters.
   */
  ScannerID scannerOpenWithScan(
    /** name of table */
    1:Text tableName,

    /** Scan instance */
    2:TScan scan,

    /** Scan attributes */
    3:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get a scanner on the current table starting at the specified row and
   * ending at the last row in the table.  Return the specified columns.
   *
   * @return scanner id to be used with other scanner procedures
   */
  ScannerID scannerOpen(
    /** name of table */
    1:Text tableName,

    /**
     * Starting row in table to scan.
     * Send "" (empty string) to start at the first row.
     */
    2:Text startRow,

    /**
     * columns to scan. If column name is a column family, all
     * columns of the specified column family are returned. It's also possible
     * to pass a regex in the column qualifier.
     */
    3:list<Text> columns,

    /** Scan attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get a scanner on the current table starting and stopping at the
   * specified rows.  ending at the last row in the table.  Return the
   * specified columns.
   *
   * @return scanner id to be used with other scanner procedures
   */
  ScannerID scannerOpenWithStop(
    /** name of table */
    1:Text tableName,

    /**
     * Starting row in table to scan.
     * Send "" (empty string) to start at the first row.
     */
    2:Text startRow,

    /**
     * row to stop scanning on. This row is *not* included in the
     * scanner's results
     */
    3:Text stopRow,

    /**
     * columns to scan. If column name is a column family, all
     * columns of the specified column family are returned. It's also possible
     * to pass a regex in the column qualifier.
     */
    4:list<Text> columns,

    /** Scan attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Open a scanner for a given prefix.  That is all rows will have the specified
   * prefix. No other rows will be returned.
   *
   * @return scanner id to use with other scanner calls
   */
  ScannerID scannerOpenWithPrefix(
    /** name of table */
    1:Text tableName,

    /** the prefix (and thus start row) of the keys you want */
    2:Text startAndPrefix,

    /** the columns you want returned */
    3:list<Text> columns,

    /** Scan attributes */
    4:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get a scanner on the current table starting at the specified row and
   * ending at the last row in the table.  Return the specified columns.
   * Only values with the specified timestamp are returned.
   *
   * @return scanner id to be used with other scanner procedures
   */
  ScannerID scannerOpenTs(
    /** name of table */
    1:Text tableName,

    /**
     * Starting row in table to scan.
     * Send "" (empty string) to start at the first row.
     */
    2:Text startRow,

    /**
     * columns to scan. If column name is a column family, all
     * columns of the specified column family are returned. It's also possible
     * to pass a regex in the column qualifier.
     */
    3:list<Text> columns,

    /** timestamp */
    4:i64 timestamp,

    /** Scan attributes */
    5:map<Text, Text> attributes
  ) throws (1:IOError io)

  /** 
   * Get a scanner on the current table starting and stopping at the
   * specified rows.  ending at the last row in the table.  Return the
   * specified columns.  Only values with the specified timestamp are
   * returned.
   *
   * @return scanner id to be used with other scanner procedures
   */
  ScannerID scannerOpenWithStopTs(
    /** name of table */
    1:Text tableName,

    /**
     * Starting row in table to scan.
     * Send "" (empty string) to start at the first row.
     */
    2:Text startRow,

    /**
     * row to stop scanning on. This row is *not* included in the
     * scanner's results
     */
    3:Text stopRow,

    /**
     * columns to scan. If column name is a column family, all
     * columns of the specified column family are returned. It's also possible
     * to pass a regex in the column qualifier.
     */
    4:list<Text> columns,

    /** timestamp */
    5:i64 timestamp,

    /** Scan attributes */
    6:map<Text, Text> attributes
  ) throws (1:IOError io)

  /**
   * Returns the scanner's current row value and advances to the next
   * row in the table.  When there are no more rows in the table, or a key
   * greater-than-or-equal-to the scanner's specified stopRow is reached,
   * an empty list is returned.
   *
   * @return a TRowResult containing the current row and a map of the columns to TCells.
   *
   * @throws IllegalArgument if ScannerID is invalid
   *
   * @throws NotFound when the scanner reaches the end
   */
  list<TRowResult> scannerGet(
    /** id of a scanner returned by scannerOpen */
    1:ScannerID id
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /**
   * Returns, starting at the scanner's current row value nbRows worth of
   * rows and advances to the next row in the table.  When there are no more 
   * rows in the table, or a key greater-than-or-equal-to the scanner's 
   * specified stopRow is reached,  an empty list is returned.
   *
   * @return a TRowResult containing the current row and a map of the columns to TCells.
   *
   * @throws IllegalArgument if ScannerID is invalid
   *
   * @throws NotFound when the scanner reaches the end
   */
  list<TRowResult> scannerGetList(
    /** id of a scanner returned by scannerOpen */
    1:ScannerID id,

    /** number of results to return */
    2:i32 nbRows
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /**
   * Closes the server-state associated with an open scanner.
   *
   * @throws IllegalArgument if ScannerID is invalid
   */
  void scannerClose(
    /** id of a scanner returned by scannerOpen */
    1:ScannerID id
  ) throws (1:IOError io, 2:IllegalArgument ia)

  /**
   * Get the regininfo for the specified row. It scans
   * the metatable to find region's start and end keys.
   *
   * @return value for specified row/column
   */
  TRegionInfo getRegionInfo(
    /** row key */
    1:Text row,

  ) throws (1:IOError io)

  /**
   * Appends values to one or more columns within a single row.
   *
   * @return values of columns after the append operation.
   */
  list<TCell> append(
    /** The single append operation to apply */
    1:TAppend append,

  ) throws (1:IOError io)

  /**
   * Atomically checks if a row/family/qualifier value matches the expected
   * value. If it does, it adds the corresponding mutation operation for put.
   *
   * @return true if the new put was executed, false otherwise
   */
  bool checkAndPut(
    /** name of table */
    1:Text tableName,

    /** row key */
    2:Text row,

    /** column name */
    3:Text column,

    /** the expected value for the column parameter, if not
        provided the check is for the non-existence of the
        column in question */
    5:Text value

    /** mutation for the put */
    6:Mutation mput,

    /** Mutation attributes */
    7:map<Text, Text> attributes
  ) throws (1:IOError io, 2:IllegalArgument ia)
}
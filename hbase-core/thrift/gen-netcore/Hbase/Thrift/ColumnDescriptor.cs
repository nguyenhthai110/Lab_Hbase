/**
 * Autogenerated by Thrift Compiler (0.12.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Collections;

using Thrift.Protocols;
using Thrift.Protocols.Entities;
using Thrift.Protocols.Utilities;
using Thrift.Transports;
using Thrift.Transports.Client;
using Thrift.Transports.Server;


namespace Hbase.Thrift
{

  /// <summary>
  /// An HColumnDescriptor contains information about a column family
  /// such as the number of versions, compression settings, etc. It is
  /// used as input when creating a table or adding a column.
  /// </summary>
  public partial class ColumnDescriptor : TBase
  {
    private byte[] _name;
    private int _maxVersions;
    private string _compression;
    private bool _inMemory;
    private string _bloomFilterType;
    private int _bloomFilterVectorSize;
    private int _bloomFilterNbHashes;
    private bool _blockCacheEnabled;
    private int _timeToLive;

    public byte[] Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    public int MaxVersions
    {
      get
      {
        return _maxVersions;
      }
      set
      {
        __isset.maxVersions = true;
        this._maxVersions = value;
      }
    }

    public string Compression
    {
      get
      {
        return _compression;
      }
      set
      {
        __isset.compression = true;
        this._compression = value;
      }
    }

    public bool InMemory
    {
      get
      {
        return _inMemory;
      }
      set
      {
        __isset.inMemory = true;
        this._inMemory = value;
      }
    }

    public string BloomFilterType
    {
      get
      {
        return _bloomFilterType;
      }
      set
      {
        __isset.bloomFilterType = true;
        this._bloomFilterType = value;
      }
    }

    public int BloomFilterVectorSize
    {
      get
      {
        return _bloomFilterVectorSize;
      }
      set
      {
        __isset.bloomFilterVectorSize = true;
        this._bloomFilterVectorSize = value;
      }
    }

    public int BloomFilterNbHashes
    {
      get
      {
        return _bloomFilterNbHashes;
      }
      set
      {
        __isset.bloomFilterNbHashes = true;
        this._bloomFilterNbHashes = value;
      }
    }

    public bool BlockCacheEnabled
    {
      get
      {
        return _blockCacheEnabled;
      }
      set
      {
        __isset.blockCacheEnabled = true;
        this._blockCacheEnabled = value;
      }
    }

    public int TimeToLive
    {
      get
      {
        return _timeToLive;
      }
      set
      {
        __isset.timeToLive = true;
        this._timeToLive = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool name;
      public bool maxVersions;
      public bool compression;
      public bool inMemory;
      public bool bloomFilterType;
      public bool bloomFilterVectorSize;
      public bool bloomFilterNbHashes;
      public bool blockCacheEnabled;
      public bool timeToLive;
    }

    public ColumnDescriptor()
    {
      this._maxVersions = 3;
      this.__isset.maxVersions = true;
      this._compression = "NONE";
      this.__isset.compression = true;
      this._inMemory = false;
      this.__isset.inMemory = true;
      this._bloomFilterType = "NONE";
      this.__isset.bloomFilterType = true;
      this._bloomFilterVectorSize = 0;
      this.__isset.bloomFilterVectorSize = true;
      this._bloomFilterNbHashes = 0;
      this.__isset.bloomFilterNbHashes = true;
      this._blockCacheEnabled = false;
      this.__isset.blockCacheEnabled = true;
      this._timeToLive = 2147483647;
      this.__isset.timeToLive = true;
    }

    public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        await iprot.ReadStructBeginAsync(cancellationToken);
        while (true)
        {
          field = await iprot.ReadFieldBeginAsync(cancellationToken);
          if (field.Type == TType.Stop)
          {
            break;
          }

          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String)
              {
                Name = await iprot.ReadBinaryAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.I32)
              {
                MaxVersions = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 3:
              if (field.Type == TType.String)
              {
                Compression = await iprot.ReadStringAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 4:
              if (field.Type == TType.Bool)
              {
                InMemory = await iprot.ReadBoolAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 5:
              if (field.Type == TType.String)
              {
                BloomFilterType = await iprot.ReadStringAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 6:
              if (field.Type == TType.I32)
              {
                BloomFilterVectorSize = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 7:
              if (field.Type == TType.I32)
              {
                BloomFilterNbHashes = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 8:
              if (field.Type == TType.Bool)
              {
                BlockCacheEnabled = await iprot.ReadBoolAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 9:
              if (field.Type == TType.I32)
              {
                TimeToLive = await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            default: 
              await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              break;
          }

          await iprot.ReadFieldEndAsync(cancellationToken);
        }

        await iprot.ReadStructEndAsync(cancellationToken);
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
    {
      oprot.IncrementRecursionDepth();
      try
      {
        var struc = new TStruct("ColumnDescriptor");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        if (Name != null && __isset.name)
        {
          field.Name = "name";
          field.Type = TType.String;
          field.ID = 1;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteBinaryAsync(Name, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.maxVersions)
        {
          field.Name = "maxVersions";
          field.Type = TType.I32;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteI32Async(MaxVersions, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (Compression != null && __isset.compression)
        {
          field.Name = "compression";
          field.Type = TType.String;
          field.ID = 3;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteStringAsync(Compression, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.inMemory)
        {
          field.Name = "inMemory";
          field.Type = TType.Bool;
          field.ID = 4;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteBoolAsync(InMemory, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (BloomFilterType != null && __isset.bloomFilterType)
        {
          field.Name = "bloomFilterType";
          field.Type = TType.String;
          field.ID = 5;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteStringAsync(BloomFilterType, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.bloomFilterVectorSize)
        {
          field.Name = "bloomFilterVectorSize";
          field.Type = TType.I32;
          field.ID = 6;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteI32Async(BloomFilterVectorSize, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.bloomFilterNbHashes)
        {
          field.Name = "bloomFilterNbHashes";
          field.Type = TType.I32;
          field.ID = 7;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteI32Async(BloomFilterNbHashes, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.blockCacheEnabled)
        {
          field.Name = "blockCacheEnabled";
          field.Type = TType.Bool;
          field.ID = 8;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteBoolAsync(BlockCacheEnabled, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (__isset.timeToLive)
        {
          field.Name = "timeToLive";
          field.Type = TType.I32;
          field.ID = 9;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteI32Async(TimeToLive, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        await oprot.WriteFieldStopAsync(cancellationToken);
        await oprot.WriteStructEndAsync(cancellationToken);
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder("ColumnDescriptor(");
      bool __first = true;
      if (Name != null && __isset.name)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Name: ");
        sb.Append(Name);
      }
      if (__isset.maxVersions)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("MaxVersions: ");
        sb.Append(MaxVersions);
      }
      if (Compression != null && __isset.compression)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Compression: ");
        sb.Append(Compression);
      }
      if (__isset.inMemory)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("InMemory: ");
        sb.Append(InMemory);
      }
      if (BloomFilterType != null && __isset.bloomFilterType)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("BloomFilterType: ");
        sb.Append(BloomFilterType);
      }
      if (__isset.bloomFilterVectorSize)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("BloomFilterVectorSize: ");
        sb.Append(BloomFilterVectorSize);
      }
      if (__isset.bloomFilterNbHashes)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("BloomFilterNbHashes: ");
        sb.Append(BloomFilterNbHashes);
      }
      if (__isset.blockCacheEnabled)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("BlockCacheEnabled: ");
        sb.Append(BlockCacheEnabled);
      }
      if (__isset.timeToLive)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("TimeToLive: ");
        sb.Append(TimeToLive);
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}
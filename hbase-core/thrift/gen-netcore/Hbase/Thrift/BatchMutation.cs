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
  /// A BatchMutation object is used to apply a number of Mutations to a single row.
  /// </summary>
  public partial class BatchMutation : TBase
  {
    private byte[] _row;
    private List<Mutation> _mutations;

    public byte[] Row
    {
      get
      {
        return _row;
      }
      set
      {
        __isset.row = true;
        this._row = value;
      }
    }

    public List<Mutation> Mutations
    {
      get
      {
        return _mutations;
      }
      set
      {
        __isset.mutations = true;
        this._mutations = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool row;
      public bool mutations;
    }

    public BatchMutation()
    {
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
                Row = await iprot.ReadBinaryAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.List)
              {
                {
                  Mutations = new List<Mutation>();
                  TList _list0 = await iprot.ReadListBeginAsync(cancellationToken);
                  for(int _i1 = 0; _i1 < _list0.Count; ++_i1)
                  {
                    Mutation _elem2;
                    _elem2 = new Mutation();
                    await _elem2.ReadAsync(iprot, cancellationToken);
                    Mutations.Add(_elem2);
                  }
                  await iprot.ReadListEndAsync(cancellationToken);
                }
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
        var struc = new TStruct("BatchMutation");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        if (Row != null && __isset.row)
        {
          field.Name = "row";
          field.Type = TType.String;
          field.ID = 1;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteBinaryAsync(Row, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (Mutations != null && __isset.mutations)
        {
          field.Name = "mutations";
          field.Type = TType.List;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          {
            await oprot.WriteListBeginAsync(new TList(TType.Struct, Mutations.Count), cancellationToken);
            foreach (Mutation _iter3 in Mutations)
            {
              await _iter3.WriteAsync(oprot, cancellationToken);
            }
            await oprot.WriteListEndAsync(cancellationToken);
          }
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
      var sb = new StringBuilder("BatchMutation(");
      bool __first = true;
      if (Row != null && __isset.row)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Row: ");
        sb.Append(Row);
      }
      if (Mutations != null && __isset.mutations)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Mutations: ");
        sb.Append(Mutations);
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}

using TokenIndex__1 = System.UInt32;
using TokenIdentifier__2 = System.String;
using TokenIdentifier__1 = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using SubAccount__1 = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using MetadataValue = System.ValueTuple<System.String, Candid.Extv2Boom.Models.MetadataValue>;
using Memo = System.Collections.Generic.List<System.Byte>;
using HeaderField__1 = System.ValueTuple<System.String, System.String>;
using Extension = System.String;
using ChunkId = System.UInt32;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AssetId = System.UInt32;
using AssetHandle__1 = System.String;
using AccountIdentifier__2 = System.String;
using AccountIdentifier__1 = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using System;

namespace Candid.Extv2Boom.Models
{
	[Variant(typeof(Result_10Tag))]
	public class Result_10
	{
		[VariantTagProperty()]
		public Result_10Tag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public Result_10(Result_10Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result_10()
		{
		}

		public static Result_10 Err(CommonError__1 info)
		{
			return new Result_10(Result_10Tag.Err, info);
		}

		public static Result_10 Ok(Metadata info)
		{
			return new Result_10(Result_10Tag.Ok, info);
		}

		public CommonError__1 AsErr()
		{
			this.ValidateTag(Result_10Tag.Err);
			return (CommonError__1)this.Value!;
		}

		public Metadata AsOk()
		{
			this.ValidateTag(Result_10Tag.Ok);
			return (Metadata)this.Value!;
		}

		private void ValidateTag(Result_10Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result_10Tag
	{
		[CandidName("err")]
		[VariantOptionType(typeof(CommonError__1))]
		Err,
		[CandidName("ok")]
		[VariantOptionType(typeof(Metadata))]
		Ok
	}
}
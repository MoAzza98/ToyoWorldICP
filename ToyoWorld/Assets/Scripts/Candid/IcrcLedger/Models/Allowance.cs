using TxIndex__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Subaccount__1 = System.Collections.Generic.List<System.Byte>;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Memo = System.Collections.Generic.List<System.Byte>;
using Balance__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcrcLedger.Models
{
	public class Allowance
	{
		[CandidName("allowance")]
		public UnboundedUInt Allowance_ { get; set; }

		[CandidName("expires_at")]
		public OptionalValue<ulong> ExpiresAt { get; set; }

		public Allowance(UnboundedUInt allowance, OptionalValue<ulong> expiresAt)
		{
			this.Allowance_ = allowance;
			this.ExpiresAt = expiresAt;
		}

		public Allowance()
		{
		}
	}
}
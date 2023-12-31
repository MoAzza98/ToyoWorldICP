using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.World.Models
{
	public class ActionState
	{
		[CandidName("actionCount")]
		public UnboundedUInt ActionCount { get; set; }

		[CandidName("actionId")]
		public string ActionId { get; set; }

		[CandidName("intervalStartTs")]
		public UnboundedUInt IntervalStartTs { get; set; }

		public ActionState(UnboundedUInt actionCount, string actionId, UnboundedUInt intervalStartTs)
		{
			this.ActionCount = actionCount;
			this.ActionId = actionId;
			this.IntervalStartTs = intervalStartTs;
		}

		public ActionState()
		{
		}
	}
}
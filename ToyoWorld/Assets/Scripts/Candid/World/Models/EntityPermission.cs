using worldId = System.String;
using groupId = System.String;
using entityId = System.String;
using configId = System.String;
using BlockIndex = System.UInt64;
using EdjCase.ICP.Candid.Mapping;

namespace Candid.World.Models
{
	public class EntityPermission
	{
		[CandidName("eid")]
		public entityId Eid { get; set; }

		[CandidName("gid")]
		public groupId Gid { get; set; }

		[CandidName("wid")]
		public worldId Wid { get; set; }

		public EntityPermission(entityId eid, groupId gid, worldId wid)
		{
			this.Eid = eid;
			this.Gid = gid;
			this.Wid = wid;
		}

		public EntityPermission()
		{
		}
	}
}
using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
	public class LogEventRequest_SCORE_EVENT : GSTypedRequest<LogEventRequest_SCORE_EVENT, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_SCORE_EVENT() : base("LogEventRequest"){
			request.AddString("eventKey", "SCORE_EVENT");
		}
		public LogEventRequest_SCORE_EVENT Set_SCORE_ATTR( long value )
		{
			request.AddNumber("SCORE_ATTR", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_SCORE_EVENT : GSTypedRequest<LogChallengeEventRequest_SCORE_EVENT, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_SCORE_EVENT() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "SCORE_EVENT");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_SCORE_EVENT SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_SCORE_EVENT Set_SCORE_ATTR( long value )
		{
			request.AddNumber("SCORE_ATTR", value);
			return this;
		}			
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_HIGH_SCORE_LB : GSTypedRequest<LeaderboardDataRequest_HIGH_SCORE_LB,LeaderboardDataResponse_HIGH_SCORE_LB>
	{
		public LeaderboardDataRequest_HIGH_SCORE_LB() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "HIGH_SCORE_LB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_HIGH_SCORE_LB (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_HIGH_SCORE_LB : GSTypedRequest<AroundMeLeaderboardRequest_HIGH_SCORE_LB,AroundMeLeaderboardResponse_HIGH_SCORE_LB>
	{
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "HIGH_SCORE_LB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_HIGH_SCORE_LB (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_HIGH_SCORE_LB : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_HIGH_SCORE_LB(GSData data) : base(data){}
		public long? SCORE_ATTR{
			get{return response.GetNumber("SCORE_ATTR");}
		}
	}
	
	public class LeaderboardDataResponse_HIGH_SCORE_LB : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_HIGH_SCORE_LB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Data_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> First_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Last_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_HIGH_SCORE_LB : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_HIGH_SCORE_LB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Data_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> First_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Last_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {


}

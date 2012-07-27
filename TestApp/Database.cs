using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

// Entity definitions

public class Players
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("nameFirst")]
    public string FirstName { get; set; }

    [Column("nameLast")]
    public string LastName { get; set; }

    [Column("bats")]
    public string Bats { get; set; }

    [Column("throws")]
    public string Throws { get; set; }

    [Column("nameNick")]
    public string NickName { get; set; }

    // TODO: how to convert this to DateTime?
    [Column("debut")]
    public long Debut { get; set; }

    // TODO: how to convert this to DateTime?
    [Column("finalGame")]
    public long FinalGame { get; set; }

    [Column("birthDay")]
    public int BirthDay { get; set; }
    
    [Column("birthMonth")]
    public int BirthMonth { get; set; }

    [Column("birthYear")]
    public int BirthYear { get; set; }

    private PlayerBattingStats _battingStats;
    private PlayerPitchingStats _pitchingStats;

    public async Task<PlayerBattingStats> GetBattingStatsAsync()
    {
        if (_battingStats == null)
        {
            var connection = await Baseball.GetConnection();
            _battingStats = await connection.GetBattingStatsAsync(PlayerID);
        }
        return _battingStats;
    }

    public async Task<PlayerPitchingStats> GetPitchingStatsAsync()
    {
        if (_pitchingStats == null)
        {
            var connection = await Baseball.GetConnection();
            _pitchingStats = await connection.GetPitchingStatsAsync(PlayerID);
        }
        return _pitchingStats;
    }
}

public class Master
{
    [Column("lahmanID")]
    public int LahmanID { get; set; }

    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("birthState")]
    public string BirthState { get; set; }

    [Column("nameFirst")]
    public string FirstName { get; set; }

    [Column("nameLast")]
    public string LastName { get; set; }
}

public class PlayerPitchingStats
{
    public List<PitchingStats> PitchingStats { get; set; }

    public PlayerPitchingStats(List<PitchingStats> pitchingStats)
    {
        PitchingStats = pitchingStats;
    }

    public int CareerWins
    {
        get
        {
            return PitchingStats.Sum(item => item.Wins);
        }
    }

    public int CareerLosses
    {
        get
        {
            return PitchingStats.Sum(item => item.Losses);
        }
    }

    public double CareerWinLossPercentage
    {
        get
        {
            return (double)CareerWins / ((double)CareerWins + (double)CareerLosses);
        }
    }

    public int CareerInningsPitchedOuts
    {
        get
        {
            return PitchingStats.Sum(item => item.InningsPitchedOuts);
        }
    }

    public double CareerEarnedRunAverage
    {
        get
        {
            double earnedRuns = (double)CareerEarnedRuns;
            return earnedRuns * 9 / CareerInningsPitched;
        }
    }

    public int CareerGames
    {
        get
        {
            return PitchingStats.Sum(item => item.Games);
        }
    }

    public int CareerGamesStarted
    {
        get
        {
            return PitchingStats.Sum(item => item.GamesStarted);
        }
    }

    /// <summary>
    /// This is a stat that tells you how many games a pitcher appeared as a reliever and completed the game in
    /// </summary>
    public int CareerGamesFinished
    {
        get
        {
            return PitchingStats.Sum(item => item.GamesFinished);
        }
    }

    public int CareerCompleteGames
    {
        get
        {
            return PitchingStats.Sum(item => item.CompleteGames);
        }
    }

    public int CareerShutouts
    {
        get
        {
            return PitchingStats.Sum(item => item.Shutouts);
        }
    }

    public int CareerSaves
    {
        get
        {
            return PitchingStats.Sum(item => item.Saves);
        }
    }

    /// <summary>
    /// This returns the innings pitched as a double that is suitable for use in computations
    /// </summary>
    public double CareerInningsPitched
    {
        get
        {
            int ipouts = PitchingStats.Sum(item => item.InningsPitchedOuts);
            return (double)ipouts / 3.0;
        }
    }

    public int CareerHits
    {
        get
        {
            return PitchingStats.Sum(item => item.Hits);
        }
    }

    public int CareerRuns
    {
        get
        {
            return PitchingStats.Sum(item => item.Runs);
        }
    }

    public int CareerEarnedRuns
    {
        get
        {
            return PitchingStats.Sum(item => item.EarnedRuns);
        }
    }

    public int CareerHomeRuns
    {
        get
        {
            return PitchingStats.Sum(item => item.HomeRuns);
        }
    }

    public int CareerWalks
    {
        get
        {
            return PitchingStats.Sum(item => item.Walks);
        }
    }

    public int CareerIntentionalWalks
    {
        get
        {
            return PitchingStats.Sum(item => item.IntentionalWalks);
        }
    }

    public int CareerStrikeouts
    {
        get
        {
            return PitchingStats.Sum(item => item.Strikeouts);
        }
    }

    public int CareerHitByPitch
    {
        get
        {
            return PitchingStats.Sum(item => item.HitByPitch);
        }
    }

    public int CareerBalks
    {
        get
        {
            return PitchingStats.Sum(item => item.Balks);
        }
    }

    public int CareerWildPitches
    {
        get
        {
            return PitchingStats.Sum(item => item.WildPitches);
        }
    }

    public int CareerBattersFaced
    {
        get
        {
            return PitchingStats.Sum(item => item.BattersFaced);
        }
    }
    public double CareerWalksPlusHitsDividedByInningsPitched
    {
        get
        {
            return ((double)CareerWalks + (double)CareerHits) / CareerInningsPitched;
        }
    }

    public double CareerHitsPerNineInnings
    {
        get
        {
            return (double)CareerHits * 9.0 / CareerInningsPitched;
        }
    }

    public double CareerHomeRunsPerNineInnings
    {
        get
        {
            return (double)CareerHomeRuns * 9.0 / CareerInningsPitched;
        }
    }

    public double CareerWalksPerNineInnings
    {
        get
        {
            return (double)CareerWalks * 9.0 / CareerInningsPitched;
        }
    }

    public double CareerStrikeoutsPerNineInnings
    {
        get
        {
            return (double)CareerStrikeouts * 9.0 / CareerInningsPitched;
        }
    }

    public double CareerStrikeoutWalkRatio
    {
        get
        {
            return (double)CareerStrikeouts / (double)CareerWalks;
        }
    }
}

/// <summary>
/// Class that encapsulates both individual batting stat entries as well
/// as cumulative totals of those stats (eg career home runs)
/// </summary>
public class PlayerBattingStats
{
    private List<BattingStats> BattingStats { get; set; }

    public PlayerBattingStats(List<BattingStats> battingStats)
    {
        BattingStats = battingStats;
    }

    public int CareerHomeRuns
    {
        get
        {
            return BattingStats.Sum(item => item.HomeRuns);
        }
    }

    public int CareerAtBats
    {
        get
        {
            return BattingStats.Sum(item => item.AtBats);
        }
    }

    public int CareerHits
    {
        get
        {
            return BattingStats.Sum(item => item.Hits);
        }
    }

    public int CareerDoubles
    {
        get
        {
            return BattingStats.Sum(item => item.Doubles);
        }
    }

    public int CareerTriples
    {
        get
        {
            return BattingStats.Sum(item => item.Triples);
        }
    }

    public double CareerWalks
    {
        get
        {
            return BattingStats.Sum(item => item.Walks);
        }
    }

    public double CareerBattingAverage
    {
        get
        {
            return (double)CareerHits / (double)CareerAtBats;
        }
    }
}

public class BattingStats
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("yearID")]
    public int Year { get; set; }

    [Column("AB")]
    public int AtBats { get; set; }

    [Column("H")]
    public int Hits { get; set; }

    [Column("2B")]
    public int Doubles { get; set; }

    [Column("3B")]
    public int Triples { get; set; }

    [Column("HR")]
    public int HomeRuns { get; set; }

    [Column("BB")]
    public int Walks { get; set; }

    [Column("SO")]
    public int StrikeOuts { get; set; }

    public double BattingAverage
    {
        get
        {
            return (double)Hits / (double)AtBats;
        }
    }
}

public class PitchingStats
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("yearID")]
    public int Year { get; set; }

    [Column("stint")]
    public int Stint { get; set; }

    [Column("W")]
    public int Wins { get; set; }

    [Column("L")]
    public int Losses { get; set; }

    [Column("G")]
    public int Games { get; set; }

    [Column("GS")]
    public int GamesStarted { get; set; }

    [Column("CG")]
    public int CompleteGames { get; set; }

    [Column("SHO")]
    public int Shutouts { get; set; }

    [Column("SV")]
    public int Saves { get; set; }

    [Column("GF")]
    public int GamesFinished { get; set; }

    [Column("IPouts")]
    public int InningsPitchedOuts { get; set; }

    [Column("H")]
    public int Hits { get; set; }

    [Column("R")]
    public int Runs { get; set; }

    [Column("ER")]
    public int EarnedRuns { get; set; }

    [Column("GIDP")]
    public int GroundIntoDoublePlays { get; set; }

    [Column("SH")]
    public int SacrificeHits { get; set; }

    [Column("SF")]
    public int SacrificeFlies { get; set; }

    [Column("HR")]
    public int HomeRuns { get; set; }

    [Column("BB")]
    public int Walks { get; set; }

    [Column("IBB")]
    public int IntentionalWalks { get; set; }

    [Column("HBP")]
    public int HitByPitch { get; set; }

    [Column("WP")]
    public int WildPitches { get; set; }

    [Column("BK")]
    public int Balks { get; set; }

    [Column("BFP")]
    public int BattersFaced { get; set; }

    [Column("SO")]
    public int Strikeouts { get; set; }

    [Column("ERA")]
    public double EarnedRunAverage { get; set; }

    // Computed values
    public double WinLossPercentage
    {
        get
        {
            return (double)Wins / ((double)Wins + (double)Losses);
        }
    }

    public double InningsPitched
    {
        get
        {
            return (double)InningsPitchedOuts / 3.0;
        }
    }

    public double WalksPlusHitsDividedByInningsPitched
    {
        get
        {
            return ((double)Walks + (double)Hits) / InningsPitched;
        }
    }

    public double HitsPerNineInnings
    {
        get
        {
            return (double)Hits * 9.0 / InningsPitched;
        }
    }

    public double HomeRunsPerNineInnings
    {
        get
        {
            return (double)HomeRuns * 9.0 / InningsPitched;
        }
    }

    public double WalksPerNineInnings
    {
        get
        {
            return (double)Walks * 9.0 / InningsPitched;
        }
    }

    public double StrikeoutsPerNineInnings
    {
        get
        {
            return (double)Strikeouts * 9.0 / InningsPitched;
        }
    }

    public double StrikeoutWalkRatio
    {
        get
        {
            return (double)Strikeouts / (double)Walks;
        }
    }
}

// AsyncLazy<T> class from Stephen Toub's blog post on combining Lazy<T> with async
// http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/10116210.aspx

public class AsyncLazy<T> : Lazy<Task<T>>
{
    public AsyncLazy(Func<T> valueFactory) : base(() => Task.Factory.StartNew(valueFactory)) { }
    public AsyncLazy(Func<Task<T>> taskFactory) : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }
    public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
}

public class Baseball
{
    const string BASEBALL_DATABASE = "baseball-archive-2011.sqlite";

    private static async Task<StorageFile> GetApplicationFolderDatabasePath()
    {
        try
        {
            var pathToApplicationDatabase = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, BASEBALL_DATABASE);
            return await StorageFile.GetFileFromPathAsync(pathToApplicationDatabase);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private static async Task<string> CopyDatabaseToApplicationFolder()
    {
        var pathToPackageDatabase = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, BASEBALL_DATABASE);
        var packageDatabase = await StorageFile.GetFileFromPathAsync(pathToPackageDatabase);
        var database = await packageDatabase.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
        return database.Path;
    }

    private static async Task<string> GetDatabasePathAsync()
    {
        StorageFile database = await GetApplicationFolderDatabasePath();
        if (database == null)
        {
            return await CopyDatabaseToApplicationFolder();
        }
        return database.Path;
    }

    static AsyncLazy<Baseball> _database = new AsyncLazy<Baseball>(async () =>
    {
        var pathToDatabase = await GetDatabasePathAsync();
        return new Baseball(new SQLiteConnection(pathToDatabase));
    });

    public static async Task<Baseball> GetConnection()
    {
        return await _database;
    }

    private SQLiteConnection _connection;

    public Baseball(SQLiteConnection connection)
    {
        _connection = connection;
    }

    private PlayerBattingStats GetBattingStats(string playerId)
    {
        var battingStats = _connection.Query<BattingStats>("select * from batting where playerId=?", playerId);
        return new PlayerBattingStats(battingStats);
    }

    public IAsyncOperation<PlayerBattingStats> GetBattingStatsAsync(string playerId)
    {
        return Task.Run(() => GetBattingStats(playerId)).AsAsyncOperation();
    }

    private PlayerPitchingStats GetPitchingStats(string playerId)
    {
        var pitchingStats = _connection.Query<PitchingStats>("select * from pitching where playerId=?", playerId);
        return new PlayerPitchingStats(pitchingStats);
    }

    public IAsyncOperation<PlayerPitchingStats> GetPitchingStatsAsync(string playerId)
    {
        return Task.Run(() => GetPitchingStats(playerId)).AsAsyncOperation();
    }

    private List<Players> GetPlayersBornInState(string stateId)
    {
        return _connection.Query<Players>("select * from master where birthState=?", stateId);
    }

    public IAsyncOperation<List<Players>> GetPlayersBornInStateAsync(string stateId)
    {
        return Task.Run(() => GetPlayersBornInState(stateId)).AsAsyncOperation();
    }

    private List<Players> GetPlayersByName(string firstName, string lastName)
    {
        return _connection.Query<Players>("select * from master where nameFirst=? and nameLast=?", firstName, lastName);
    }

    public IAsyncOperation<List<Players>> GetPlayersByNameAsync(string firstName, string lastName)
    {
        return Task.Run(() => GetPlayersByName(firstName, lastName)).AsAsyncOperation();
    }

    private Players GetPlayerById(string playerId)
    {
        var players = _connection.Query<Players>("select * from master where playerId=?", playerId);
        if (players.Count != 1)
        {
            return null;
        }
        else
        {
            return players[0];
        }
    }

    public IAsyncOperation<Players> GetPlayerByIdAsync(string playerId)
    {
        return Task.Run(() => GetPlayerById(playerId)).AsAsyncOperation();
    }
}

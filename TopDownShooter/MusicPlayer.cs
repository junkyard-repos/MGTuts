using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TopDownShooter
{
    public class MusicPlayer
    {
        public const int NUM_SONGS = 14;

        public const int SONG_TITLE_MENU = 0;

        public const int SONG_SECTOR_MAP = 1;

        public const int SONG_DESERT = 2;
        public const int SONG_FOREST = 3;
        public const int SONG_OCEAN = 4;
        public const int SONG_BARREN = 5;
        public const int SONG_ICE = 6;
        public const int SONG_LAVA = 7;
        public const int SONG_MINING = 8;
        public const int SONG_ASTEROID_BELT = 9;
        public const int SONG_ENCOUNTER_HUMAN = 10;
        public const int SONG_ENCOUNTER_CYBORG = 11;
        public const int SONG_ENCOUNTER_AI = 12;
        public const int SONG_ENCOUNTER_ALIEN = 13;

        private FMOD.System FMODSystem;
        private FMOD.Channel Channel;
        private FMOD.Sound[] Songs;
        private FMOD.RESULT result;
        private FMOD.ChannelGroup ChannelGroup;

        private static MusicPlayer _instance;

        public static MusicPlayer Instance { get { return _instance; } }

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Init()
        {
            if (Environment.Is64BitProcess)
                LoadLibrary(System.IO.Path.GetFullPath("FMOD\\fmod.dll"));
            else
                LoadLibrary(System.IO.Path.GetFullPath("FMOD\\32\\fmod.dll"));

            _instance = new MusicPlayer();
        }

        public void Unload()
        {
            FMODSystem.release();
        }

        private MusicPlayer()
        {
            result = FMOD.Factory.System_Create(out FMODSystem);

            if (result != FMOD.RESULT.OK)
            {
                throw new Exception("This crap didn't work!!");
            }

            result = FMODSystem.setDSPBufferSize(1024, 10);
            result = FMODSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)0);

            var info = new FMOD.CREATESOUNDEXINFO();
            var song = new FMOD.Sound();

            ChannelGroup = new FMOD.ChannelGroup();
            ChannelGroup.clearHandle();

            result = FMODSystem.createStream("rain.ogg", FMOD.MODE.DEFAULT, out song);

            result = FMODSystem.playSound(song, ChannelGroup, false, out Channel);

            

            bool isPlaying = false;

            Channel.isPlaying(out isPlaying);

            Channel.setVolume(1);
            Channel.setMode(FMOD.MODE.LOOP_NORMAL);
            Channel.setLoopCount(-1);

            int t = 1;

        }

        //private void LoadSong(int songId, string name)
        //{
        //    FMOD.RESULT r = FMODSystem.createStream("Content/Music/rain.oog", FMOD.MODE.DEFAULT, out Songs[songId]);
        //    //Console.WriteLine("loading " + songId + ", got result " + r);
        //}

        //private int _current_song_id;

        //public bool IsPlaying()
        //{
        //    bool isPlaying = false;

        //    if (Channel != null)
        //        Channel.isPlaying(out isPlaying);

        //    return isPlaying;
        //}

        //public void Play(int songId)
        //{
        //    Console.WriteLine("Play(" + songId + ")");

        //    if (_current_song_id != songId)
        //    {
        //        Stop();

        //        if (songId >= 0 && songId < NUM_SONGS && Songs[songId] != null)
        //        {
        //            FMODSystem.playSound(Songs[songId], null, false, out Channel);
        //            UpdateVolume();
        //            Channel.setMode(FMOD.MODE.LOOP_NORMAL);
        //            Channel.setLoopCount(-1);

        //            _current_song_id = songId;
        //        }
        //    }
        //}

        //public void UpdateVolume()
        //{
        //    //if (Channel != null)
        //    Channel.setVolume(10 / 100f);
        //}

        //public void Stop()
        //{
        //    if (IsPlaying())
        //        Channel.stop();

        //    _current_song_id = -1;
        //}
    }
}

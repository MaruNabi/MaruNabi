using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feel
{
	/// <summary>
	/// A small script used to power the FeelMMSoundManagerPlaylistManager demo scene
	/// </summary>
	public class PlaylistDemo : MonoBehaviour
	{
		/// the playlist manager to read data on
		public MMSMPlaylistManager PlaylistManager;
		/// a progress bar meant to display the progress of the song currently playing 
		public MMProgressBar ProgressBar;
		/// the name of the song currently playing
		public TMP_Text SongName;
		/// a text displaying the current progress of the song in minutes/seconds 
		public TMP_Text SongDuration;

		/// <summary>
		/// On Update, updates the progress bar and song duration counter
		/// </summary>
		protected virtual void Update()
		{
			if (PlaylistManager.CurrentClipDuration == 0f)
			{
				ProgressBar.SetBar(0f, 0f, 1f);
			}
			else
			{
				ProgressBar.SetBar(PlaylistManager.CurrentTime, 0f, PlaylistManager.CurrentClipDuration);
				SongDuration.text = MMTime.FloatToTimeString(PlaylistManager.CurrentTime, false, true, true, false)
				                    + " / "
				                    + MMTime.FloatToTimeString(PlaylistManager.CurrentClipDuration, false, true, true, false);
			}
		}

		/// <summary>
		/// Updates the song name display
		/// </summary>
		protected virtual void UpdateSongName()
		{
			int displayIndex = PlaylistManager.CurrentSongIndex + 1;
			SongName.text = displayIndex + ". " + PlaylistManager.CurrentSongName;
		}
		
		/// <summary>
		/// When a new song starts to play, we update its name
		/// </summary>
		/// <param name="channel"></param>
		protected virtual void OnPlayEvent(int channel)
		{
			UpdateSongName();
		}
		
		/// <summary>
		/// Starts listening for events
		/// </summary>
		protected virtual void OnEnable()
		{
			MMPlaylistNewSongStartedEvent.Register(OnPlayEvent);
		}
		
		/// <summary>
		/// Stops listening for events
		/// </summary>
		protected virtual void OnDisable()
		{
			MMPlaylistNewSongStartedEvent.Unregister(OnPlayEvent);
		}
	}
}

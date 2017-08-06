using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;

using RedRunner.Characters;
using RedRunner.Collectables;
using RedRunner.TerrainGeneration;

namespace RedRunner
{

	public sealed class GameManager : MonoBehaviour
	{

		public delegate void CoinChangedHandler ( int coin );

		public delegate void AudioEnabledHandler ( bool active );

		public delegate void ScoreHandler ( float newScore, float highScore, float lastScore );

		public delegate void ResetHandler ();

		public static event ResetHandler OnReset;
		public static event ScoreHandler OnScoreChanged;
		public static event CoinChangedHandler OnCoinChanged;
		public static event AudioEnabledHandler OnAudioEnabled;

		private static GameManager m_Singleton;

		public static GameManager Singleton
		{
			get
			{
				return m_Singleton;
			}
		}

		[SerializeField]
		private Character m_MainCharacter;
		[SerializeField]
		[TextArea ( 3, 30 )]
		private string m_ShareText;
		[SerializeField]
		private string m_ShareUrl;
		[SerializeField]
		private LoadEvent m_OnLoaded;
		private float m_StartScoreX = 0f;
		private float m_HighScore = 0f;
		private float m_LastScore = 0f;
		private float m_Score = 0f;

		private int m_Coin = 0;
		private bool m_GameStarted = false;
		private bool m_GameRunning = false;
		private bool m_AudioEnabled = true;

		public int coin
		{
			get
			{
				return m_Coin;
			}
		}

		public bool gameStarted
		{
			get
			{
				return m_GameStarted;
			}
		}

		public bool gameRunning
		{
			get
			{
				return m_GameRunning;
			}
		}

		public bool audioEnabled
		{
			get
			{
				return m_AudioEnabled;
			}
		}

		void Awake ()
		{
			if ( m_Singleton != null )
			{
				Destroy ( gameObject );
				return;
			}
			SaveGame.Serializer = new SaveGameBinarySerializer ();
			m_Singleton = this;
			EndGame ();
			m_Score = 0f;
			Coin.OnCoinCollected += Coin_OnCoinCollected;
			if ( SaveGame.Exists ( "coin" ) )
			{
				m_Coin = SaveGame.Load<int> ( "coin" );
			}
			else
			{
				m_Coin = 0;
			}
			if ( SaveGame.Exists ( "audioEnabled" ) )
			{
				SetAudioEnabled ( SaveGame.Load<bool> ( "audioEnabled" ) );
			}
			else
			{
				SetAudioEnabled ( true );
			}
			if ( SaveGame.Exists ( "lastScore" ) )
			{
				m_LastScore = SaveGame.Load<float> ( "lastScore" );
			}
			else
			{
				m_LastScore = 0f;
			}
			if ( SaveGame.Exists ( "highScore" ) )
			{
				m_HighScore = SaveGame.Load<float> ( "highScore" );
			}
			else
			{
				m_HighScore = 0f;
			}
			if ( OnCoinChanged != null )
			{
				OnCoinChanged ( m_Coin );
			}
			m_MainCharacter.OnDead += MainCharacter_OnDead;
			m_StartScoreX = m_MainCharacter.transform.position.x;
		}

		void MainCharacter_OnDead ()
		{
			m_LastScore = m_Score;
			if ( m_Score > m_HighScore )
			{
				m_HighScore = m_Score;
			}
			if ( OnScoreChanged != null )
			{
				OnScoreChanged ( m_Score, m_HighScore, m_LastScore );
			}
		}

		void Start ()
		{
			StartCoroutine ( Load () );
		}

		void Update ()
		{
			if ( m_GameRunning )
			{
				if ( m_MainCharacter.transform.position.x > m_StartScoreX && m_MainCharacter.transform.position.x > m_Score )
				{
					m_Score = m_MainCharacter.transform.position.x;
					if ( OnScoreChanged != null )
					{
						OnScoreChanged ( m_Score, m_HighScore, m_LastScore );
					}
				}
			}
		}

		IEnumerator Load ()
		{
			yield return new WaitForSecondsRealtime ( 3f );
			m_OnLoaded.Invoke ();
		}

		void OnApplicationQuit ()
		{
			if ( m_Score > m_HighScore )
			{
				m_HighScore = m_Score;
			}
			SaveGame.Save<int> ( "coin", m_Coin );
			SaveGame.Save<float> ( "lastScore", m_Score );
			SaveGame.Save<float> ( "highScore", m_HighScore );
		}

		void Coin_OnCoinCollected ( Coin coin )
		{
			m_Coin++;
			if ( OnCoinChanged != null )
			{
				OnCoinChanged ( m_Coin );
			}
		}

		public void ExitGame ()
		{
			Application.Quit ();
		}

		public void ToggleAudioEnabled ()
		{
			SetAudioEnabled ( !m_AudioEnabled );
		}

		public void SetAudioEnabled ( bool active )
		{
			m_AudioEnabled = active;
			AudioListener.volume = active ? 1f : 0f;
			if ( OnAudioEnabled != null )
			{
				OnAudioEnabled ( active );
			}
		}

		public void StartGame ()
		{
			m_GameStarted = true;
			ResumeGame ();
		}

		public void StopGame ()
		{
			m_GameRunning = false;
			Time.timeScale = 0f;
		}

		public void ResumeGame ()
		{
			m_GameRunning = true;
			Time.timeScale = 1f;
		}

		public void EndGame ()
		{
			m_GameStarted = false;
			StopGame ();
		}

		public void RespawnMainCharacter ()
		{
			RespawnCharacter ( m_MainCharacter );
		}

		public void RespawnCharacter ( Character character )
		{
			Block block = TerrainGenerator.Singleton.GetCharacterBlock ();
			if ( block != null )
			{
				Vector3 position = block.transform.position;
				position.y += 2.56f;
				position.x += 1.28f;
				character.transform.position = position;
				character.Reset ();
			}
		}

		public void Reset ()
		{
			m_Score = 0f;
			if ( OnReset != null )
			{
				OnReset ();
			}
		}

		public void ShareOnTwitter ()
		{
			Share ( "https://twitter.com/intent/tweet?text={0}&url={1}" );
		}

		public void ShareOnGooglePlus ()
		{
			Share ( "https://plus.google.com/share?text={0}&href={1}" );
		}

		public void ShareOnFacebook ()
		{
			Share ( "https://www.facebook.com/sharer/sharer.php?u={1}" );
		}

		public void Share ( string url )
		{
			Application.OpenURL ( string.Format ( url, m_ShareText, m_ShareUrl ) );
		}

		[System.Serializable]
		public class LoadEvent : UnityEvent
		{
			
		}

	}

}
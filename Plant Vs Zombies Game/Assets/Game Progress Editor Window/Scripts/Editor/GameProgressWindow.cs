using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameProgressWindow : EditorWindow
{
	public Dictionary<string, string> objectives;
	public List<bool> objectivesCompleted;

	public string playerPrefsName = "Game Progress Window";

	bool showAddObjectivesMenu = false;
	bool showObjectives = true;

	//Private variables
	string objecttiveName = "Objective Name";
	string objecttiveDescription = "Objective Description";

	Vector2 scrollPos;

	bool fold;

	[MenuItem("Window/Game Progress Window")]
	public static void ShowWindow()
	{
		GetWindow<GameProgressWindow>("Game Progress");
	}

	private void Awake()
	{
		objectives = new Dictionary<string, string>();
		objectivesCompleted = new List<bool>();
	}

	private void OnFocus()
	{
		playerPrefsName = PlayerPrefs.GetString("PrefsName");
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal();
		playerPrefsName = GUILayout.TextField(playerPrefsName);

		if (GUILayout.Button("Update Player Prefs Name"))
		{
			PlayerPrefs.SetString("PrefsName", playerPrefsName);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		GUIContent f = new GUIContent();
		f.text = "Show Add New Objectives Menu";
		showAddObjectivesMenu = EditorGUILayout.BeginFoldoutHeaderGroup(showAddObjectivesMenu, f);

		if (showAddObjectivesMenu)
		{
			GUILayout.Space(5);
			//Show Add Objectives Components
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name ");
			objecttiveName = GUILayout.TextField(objecttiveName);
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical();
			GUILayout.Label("Description ");
			objecttiveDescription = GUILayout.TextArea(objecttiveDescription);
			GUILayout.EndVertical();

			GUILayout.Space(5);
			if (GUILayout.Button("Add New Objective"))
			{
				PlayerPrefs.SetString(playerPrefsName, PlayerPrefs.GetString(playerPrefsName) + ">" + objecttiveName.Replace(":", "|").Replace(">", "~") + ":" + objecttiveDescription.Replace(":", "|").Replace(">", "~") + ":false");
			}
		}

		EditorGUILayout.EndFoldoutHeaderGroup();

		List<GUILayoutOption> options = new List<GUILayoutOption>();

		//Showing Objectives

		showObjectives = EditorGUILayout.BeginToggleGroup("Show Objectives", showObjectives);
		
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		
		if (showObjectives)
		{
			if (PlayerPrefs.GetString(playerPrefsName) != "")
			{
				objectives = new Dictionary<string, string>();
				objectivesCompleted = new List<bool>();

				string[] values = PlayerPrefs.GetString(playerPrefsName).Split(">"[0]);

				int i = 0;
				foreach (var item in values)
				{
					if (item.Contains(":"))
					{
						objectives.Add(item.Split(":"[0])[0], item.Split(":"[0])[1]);

						options.Add(GUILayout.ExpandWidth(true));
						options.Add(GUILayout.ExpandHeight(false));

						GUIContent d = new GUIContent();
						d.text = item.Split(":"[0])[0].Replace("|", ":").Replace("~", ">");

						GUIContent dd = new GUIContent();
						dd.text = item.Split(":"[0])[1].Replace("|", ":").Replace("~", ">");

						objectivesCompleted.Add(bool.Parse(item.Split(":"[0])[2]));

						GUILayout.Box(d, options.ToArray());

						GUILayout.Box(dd, options.ToArray());

						GUIContent g = new GUIContent();
						g.text = "Show More Options";

						bool foldd = bool.Parse(PlayerPrefs.GetString("boolValue" + i) == "" ? "true" : PlayerPrefs.GetString("boolValue" + i));

						foldd = EditorGUILayout.BeginFoldoutHeaderGroup(foldd, g);
						PlayerPrefs.SetString("boolValue" + i, foldd + "");

						if (foldd)
						{
							GUILayout.BeginHorizontal();

							if (!objectivesCompleted[i])
							{
								if (GUILayout.Button("Mark As Finished"))
								{
									objectivesCompleted[i] = true;
								}
							}
							else
							{
								if (GUILayout.Button("Mark As UnFinished"))
								{
									objectivesCompleted[i] = false;
								}
							}

							if (GUILayout.Button("Remove"))
							{
								objectives.Remove(item.Split(":"[0])[0]);
							}

							GUILayout.EndHorizontal();
						}

						EditorGUILayout.EndFoldoutHeaderGroup();

						GUILayout.Space(15);

						i++;
					}
				}
			}

			//Update player prefs
			int ii = 0;
			PlayerPrefs.SetString(playerPrefsName, "");
			foreach (var item in objectives)
			{
				PlayerPrefs.SetString(playerPrefsName, PlayerPrefs.GetString(playerPrefsName) + ">" + item.Key + ":" + item.Value + ":" + objectivesCompleted[ii]);

				ii++;
			}

			objectives = new Dictionary<string, string>();
			objectivesCompleted = new List<bool>();
		}
		
		GUILayout.EndScrollView();
		
		EditorGUILayout.EndToggleGroup();
	}
}

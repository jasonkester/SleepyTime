using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace SleepyTime
{
	[BepInPlugin("PartyWhale.SleepyTime", "SleepyTime", "1.0.0")]
	//[BepInProcess("valheim.exe")]
    public class Mod : BaseUnityPlugin
	{
		private readonly Harmony harmony = new Harmony("PartyWhale.SleepyTime");

		void Awake()
		{
			harmony.PatchAll();
		}

		[HarmonyPatch(typeof(Game), "EverybodyIsTryingToSleep")]
		class EverybodyIsTryingToSleep_Patch
		{
			static bool Prefix(ref bool __result)
			{
				List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();
				//Debug.Log($"in EverybodyIsTryingToSleep ({allCharacterZDOS.Count} online)...");
				if (allCharacterZDOS.Count == 0)
				{
					__result = false;
					return false;
				}

				int inBedCount = 0;
				foreach (ZDO item in allCharacterZDOS)
				{
					if (item.GetBool("inBed"))
					{
						inBedCount++;
					}
				}

				if (inBedCount > 0)
				{
					Debug.Log($"{inBedCount} out of {allCharacterZDOS.Count} characters sleeping (need at least {allCharacterZDOS.Count / 2})");

					if (inBedCount >= (allCharacterZDOS.Count / 2))
					{
						Debug.Log($"Close enough.  Let's sleep!");
						__result = true;
					}

				}



				// skip original method (since we've pulled the whole thing in here)
				return false;
			}
		}

		// [HarmonyPatch(typeof(Game), "UpdateSleeping")]
		// class UpdateSleeping_Patch
		// {
		// 	static void Prefix()
		// 	{
		// 		Debug.Log($"in UpdateSleeping. Is server? {ZNet.instance.IsServer()}");
		// 	}
		// }

	}
}

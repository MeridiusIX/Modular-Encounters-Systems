using System;
using System.Text.RegularExpressions;

namespace ModularEncountersSystems.Spawning.Manipulation {

	public static class RandomNameGenerator {

		public static Random Rnd = new Random();

		public static string CharStringAll = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		public static string CharStringNumbers = "0123456789";
		public static string CharStringLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static string[] GoodAdjectives = {

			"Gilded",
			"Privilaged",
			"Honorable",
			"Heroic",
			"Brave",
			"Giddy",
			"Happy",
			"Kind",
			"Determined",
			"Excited",
			"Satisfied",
			"Patriotic",
			"Wishful"
		};
		public static string[] NeutralAdjectives = {

			"Cautious",
			"Timid",
			"Stubborn",
			"Bored",
			"Stoic",
			"Shivering",
			"Sweating",
			"Lonely",
			"Lone"
		};
		public static string[] BadAdjectives = {

			"Dishonorable",
			"Filthy",
			"Broken",
			"Shunned",
			"Corrupt",
			"Contaminated",
			"Compromised",
			"Cowardly",
			"Gloomy",
			"Sour",
			"Problematic",
			"Morose",
			"Glum",
			"Grumpy",
			"Sneaky",
			"Rude",
			"Enraged",
			"Insane",
			"Twisted",
			"Unwelcome",
			"Wicked"
		};

		public static string[] FunnyAdjectives = {

			"Goofy",
			"Wacky",
			"Lewd",
			"Silly",
			"Derpy",
			"Hyper",
			"Jittery",
			"Jumping",
			"Lustful",
			"Whimsical",
			"Horny",
			"Whiny"
		};

		public static string[] ColorAdjectives = {

			"Red",
			"Orange",
			"Yellow",
			"Green",
			"Teal",
			"Blue",
			"Purple",
			"Gold",
			"Silver",
			"Dark",
			"Light",
			"Black",
			"Grey",
			"White"

		};

		public static string[] GoodNouns = {

			"Victory",
			"Triumph",
			"Freedom",
			"Pride",
			"Success",
			"Accomplishment",
			"Zeal",
			"Devotion",
			"Motivation",
			"Vigor",
			"Benevolence",
			"Morality",
			"Love",
			"Light",
			"Ressurection",
			"Virtue",
			"Integrity",
			"Honor",
			"Dignity",
			"Purity",
			"Fortitude",
			"Discipline",
			"Redemption",
			"Advantage"

		};

		public static string[] NeutralNouns = {

			"Caution",
			"Discretion",
			"Guidance",
			"Persuasion",
			"Warning",
			"Stoicism",
			"Tolerance",
			"Resignation",
			"Vision",
			"Sacrifice",
			"Confusion",
			"Bewilderment",
			"Concern",
			"Aura",
			"Era",
			"Eon",
			"Epoch",
			"Climate"

		};

		public static string[] BadNouns = {

			"Malice",
			"Greed",
			"Wrath",
			"Gluttony",
			"Lust",
			"Spite",
			"Anger",
			"Envy",
			"Corruption",
			"Malevolence",
			"Revenge",
			"Animus",
			"Bitterness",
			"Grudge",
			"Hatred",
			"Hostility",
			"Loathing",
			"Belligerence",
			"Suffering",
			"Nightmare",
			"Darkness",
			"Plague",
			"Vice"

		};



		public static string[] FunnyNouns = {

			"Trickery",
			"Stench",
			"Aroma",
			"Left-Overs",
			"Dirty-Laundry",
			"Private-Stash"

		};

		public static string[] AuthorityNouns = {

			"Ambassador",
			"Diplomat",
			"Senator",
			"Dictator",
			"Autocrat",
			"Emperor",
			"Monarch",
			"Oligarch",
			"Advisor",
			"Politician",
			"Mayor",
			"President",
			"Minister",
			"Bishop",
			"Administrator"
		};

		public static string[] MilitaryNouns = {

			"Officer",
			"General",
			"Admiral",
			"Soldier",
			"Captain",
			"Private",
			"Commodore",
			"Commander",
			"Cadet",
			"Lieutenant",
			"Sergeant",
			"Major",
			"Corporal"

		};

		public static string[] BaddieNouns = {

			"Scoundrel",
			"Abductor",
			"Marauder",
			"Smuggler",
			"Murderer",
			"Thief",
			"Vagrant",
			"Aggressor",
			"Villain",
			"Thug",
			"Mugger",
			"Spy",
			"Infiltrator",
			"Violator",
			"Decimator",
			"Vandal",
			"Arsonist",
			"Saboteur",
			"Malcontent",
			"Interloper"
		};

		public static string[] ExplorerNouns = {

			"Explorer",
			"Voyager",
			"Wanderer",
			"Seeker",
			"Adventurer",
			"Nomad",
			"Scavenger",
			"Prospector",
			"Navigator"
		};

		public static string[] JobNouns = {

			"Courier",
			"Mechanic",
			"Engineer",
			"Artist",
			"Merchant",
			"Programmer",
			"Scientist",
			"Chemist",
			"Biologist",
			"Doctor",
			"Nurse",
			"Janitor"
		};

		public static string[] BirdNouns = {

			"Pigeon",
			"Seagull",
			"Robin",
			"Starling",
			"Goose",
			"Chicken",
			"Turkey",
			"Osterich",
			"Duck",
			"Falcon",
			"Hawk",
			"Eagle",
			"Heron",
			"Vulture",
			"Woodpecker"

		};
		public static string[] AnimalNouns = {

			"Buffalo",
			"Rhinoceros",
			"Moose",
			"Stag",
			"Bear",
			"Snake",
			"Elephant",
			"Mammoth",
			"Tiger",
			"Lion",
			"Panther",
			"Cheetah",
			"Cougar",
			"Wolf",
			"Coyote",
			"Mule",
			"Mouse",
			"Badger",
			"Hound"

		};
		public static string[] FishNouns = {

			"Whale",
			"Shark",
			"Piranha",
			"Sword-Fish",
			"Octopus",
			"Squid",
			"Tuna",
			"Salmon",
			"Lobster",
			"Crab",
			"Mollusk",
			"Clam",
			"Barracuda",
			"Sturgeon",
			"Pickerel",
			"Eel",
			"Stingray",
			"Remora",
			"Manta",
			"Shrimp",
			"Prawn",
			"Koi",
			"Megalodon"

		};

		public static string[] InsectNouns = {

			"Bee",
			"Hornet",
			"Wasp",
			"Dragonfly",
			"Mantis",
			"Butterfly",
			"Moth",
			"Ant",
			"Cockroach",
			"Beetle",
			"Earwig",
			"Centipede",
			"Spider",
			"Scorpion",
			"Grasshopper",
			"Cricket",
			"Tarantula",
			"Mosquito"

		};

		public static string[] Surnames = {

			"Smith",
			"Johnson",
			"Williams",
			"Davidson",
			"Wilson",
			"Blanchard",
			"Gallant",
			"Moran",
			"Gonzalez",
			"Garrett",
			"Boudreau",
			"Cormier",
			"Parker",
			"Simpson",
			"Griffin",
			"Murphy",
			"Morrell",
			"Rogers",
			"Adams",
			"Van Luven",
			"Donnelley",
			"Allen",
			"Atkins",
			"Anderson",
			"Armstrong",
			"Bailey",
			"Banks",
			"Barnes",
			"Baxter",
			"Benson",
			"Masterson",
			"Massey",
			"Oakford",
			"Savoie",
			"Warner",
			"Compton",
			"Hill",
			"Henderson",
			"Paulsen",
			"Cobert",
			"Tillerman",
			"Samuels",
			"Jackson",
			"Lawrence",
			"Manning",
			"Hendricks",
			"Dunn",
			"Barton",
			"Steeves",
			"Wilkins",
			"Harding",
			"Langille",
			"Lancaster"

		};

		public static string[] NatoLetter = {

			"Alpha",
			"Bravo",
			"Charlie",
			"Delta",
			"Echo",
			"Foxtrot",
			"Golf",
			"Hotel",
			"India",
			"Juliett",
			"Kilo",
			"Lima",
			"Mike",
			"November",
			"Oscar",
			"Papa",
			"Quebec",
			"Romeo",
			"Sierra",
			"Tango",
			"Uniform",
			"Victor",
			"Whiskey",
			"X-Ray",
			"Yankee",
			"Zulu"

		};

		public static string[] GreekLetter = {

			"Alpha",
			"Beta",
			"Gamma",
			"Delta",
			"Epsilon",
			"Zeta",
			"Eta",
			"Theta",
			"Iota",
			"Kappa",
			"Lambda",
			"Mu",
			"Nu",
			"Xi",
			"Omicron",
			"Pi",
			"Rho",
			"Sigma",
			"Tau",
			"Upsilon",
			"Phi",
			"Chi",
			"Psi",
			"Omega"

		};

		public static string ProcessGridname(string pattern, string gridName){
			string newPattern = pattern;
			string keyword = "DefaultGridName";
			
			// Prevent recursion
			if (gridName.Contains(keyword)){
				return newPattern;
			}

			newPattern = ProcessString(newPattern, keyword, (new string [] {gridName}) );

			return newPattern;
		}
		public static string CreateRandomNameFromPattern(string pattern) {

			string newPattern = pattern;

			newPattern = ProcessString(newPattern, "GoodAdjective", GoodAdjectives);
			newPattern = ProcessString(newPattern, "NeutralAdjective", NeutralAdjectives);
			newPattern = ProcessString(newPattern, "BadAdjective", BadAdjectives);
			newPattern = ProcessString(newPattern, "FunnyAdjective", FunnyAdjectives);
			newPattern = ProcessString(newPattern, "ColorAdjective", ColorAdjectives);
			newPattern = ProcessString(newPattern, "GoodNoun", GoodNouns);
			newPattern = ProcessString(newPattern, "NeutralNoun", NeutralNouns);
			newPattern = ProcessString(newPattern, "BadNoun", BadNouns);
			newPattern = ProcessString(newPattern, "FunnyNoun", FunnyNouns);

			newPattern = ProcessString(newPattern, "AuthorityNoun", AuthorityNouns, true);
			newPattern = ProcessString(newPattern, "MilitaryNoun", MilitaryNouns, true);
			newPattern = ProcessString(newPattern, "BaddieNoun", BaddieNouns, true);
			newPattern = ProcessString(newPattern, "ExplorerNoun", ExplorerNouns, true);
			newPattern = ProcessString(newPattern, "JobNoun", JobNouns, true);
			newPattern = ProcessString(newPattern, "BirdNoun", BirdNouns, true);
			newPattern = ProcessString(newPattern, "AnimalNoun", AnimalNouns, true);
			newPattern = ProcessString(newPattern, "FishNoun", FishNouns, true);
			newPattern = ProcessString(newPattern, "InsectNoun", InsectNouns, true);
			newPattern = ProcessString(newPattern, "SurnamesNoun", Surnames, true);

			newPattern = ProcessCharString(newPattern, "RandomLetter", CharStringLetters);
			newPattern = ProcessCharString(newPattern, "RandomNumber", CharStringNumbers);
			newPattern = ProcessCharString(newPattern, "RandomChar", CharStringAll);

			newPattern = ProcessString(newPattern, "NatoLetter", NatoLetter);
			newPattern = ProcessString(newPattern, "GreekLetter", GreekLetter);

			return newPattern;

		}

		public static string ProcessString(string existingString, string keyword, string[] wordArray, bool handleOwnership = false) {

			if (!existingString.Contains(keyword))
				return existingString;

			string result = existingString;

			while (result.Contains(keyword)) {

				var randString = wordArray[Rnd.Next(0, wordArray.Length)];

				if (handleOwnership) {

					if (randString.EndsWith("s") && result.Contains(keyword + "'s")) {

						result = ReplaceFirstOccurence(keyword + "'s", randString + "'", result);

					} else {

						result = ReplaceFirstOccurence(keyword, randString, result);

					}

				} else {

					result = ReplaceFirstOccurence(keyword, randString, result);

				}

			}

			return result;

		}

		public static string ProcessCharString(string existingString, string keyword, string wordArray, bool handleOwnership = false) {

			if (!existingString.Contains(keyword))
				return existingString;

			string result = existingString;

			while (result.Contains(keyword)) {

				var randString = wordArray[Rnd.Next(0, wordArray.Length)].ToString();

				if (handleOwnership) {

					if (randString.EndsWith("s") && result.Contains(keyword + "'s")) {

						result = ReplaceFirstOccurence(keyword + "'s", randString + "'", result);

					} else {

						result = ReplaceFirstOccurence(keyword, randString, result);

					}

				} else {

					result = ReplaceFirstOccurence(keyword, randString, result);

				}

			}

			return result;

		}

		//Following Method Found At This Source:
		//https://social.msdn.microsoft.com/Forums/en-US/25936e13-6ae6-4234-b604-d68d3c798d68/replace-only-first-instance-of-each?forum=regexp
		public static string ReplaceFirstOccurence(string wordToReplace, string replaceWith, string input) {

			Regex r = new Regex(wordToReplace, RegexOptions.IgnoreCase);

			return r.Replace(input, replaceWith, 1);

		}


	}

}

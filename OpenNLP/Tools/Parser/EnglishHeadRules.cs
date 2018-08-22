//Copyright (C) 2005 Richard J. Northedge
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

//This file is based on the EnglishHeadRules.java source file found in the
//original java implementation of OpenNLP.  That source file contains the following header:

//Copyright (C) 2003 Thomas Morton
// 
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
// 
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.
// 
//You should have received a copy of the GNU Lesser General Public
//License along with this program; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.IO;
using System.Collections.Generic;
using OpenNLP.Tools.Trees;

namespace OpenNLP.Tools.Parser
{
	/// <summary> 
	/// Class for storing the English head rules associated with parsing. 
	/// </summary>
	public class EnglishHeadRules : IHeadRules
	{
		private Dictionary<string, HeadRule> mHeadRules;
		
		public EnglishHeadRules(string ruleFile)
		{
			ReadHeadRules(ruleFile);
		}
		
		public virtual Parse GetHead(Parse[] constituents, string type)
		{
			if (constituents[0].Type == MaximumEntropyParser.TokenNode)
			{
				return null;
			}
            if (type == CoordinationTransformer.Noun || type == AbstractCollinsHeadFinder.NX)
            {
                var tags1 = new string[]
                {
                    PartsOfSpeech.NounSingularOrMass, PartsOfSpeech.ProperNounSingular, PartsOfSpeech.ProperNounPlural,
                    PartsOfSpeech.NounPlural, AbstractCollinsHeadFinder.NX, PartsOfSpeech.AdjectiveComparative,
                    PartsOfSpeech.PossessiveEnding
                };
				for (int currentConstituent = constituents.Length - 1; currentConstituent >= 0; currentConstituent--)
				{
					for (int currentTag = tags1.Length - 1; currentTag >= 0; currentTag--)
					{
						if (constituents[currentConstituent].Type.Equals(tags1[currentTag]))
						{
							return (constituents[currentConstituent].Head);
						}
					}
				}
				for (int currentConstituent = 0; currentConstituent < constituents.Length; currentConstituent++)
				{
                    if (constituents[currentConstituent].Type.Equals(CoordinationTransformer.Noun))
					{
						return (constituents[currentConstituent].Head);
					}
				}
                var tags2 = new string[] { PartsOfSpeech.DollarSign, CoordinationTransformer.Adjective, AbstractCollinsHeadFinder.PRN };
				for (int currentConstituent = constituents.Length - 1; currentConstituent >= 0; currentConstituent--)
				{
					for (int currentTag = tags2.Length - 1; currentTag >= 0; currentTag--)
					{
						if (constituents[currentConstituent].Type.Equals(tags2[currentTag]))
						{
							return (constituents[currentConstituent].Head);
						}
					}
				}
                var tags3 = new string[] { PartsOfSpeech.Adjective, PartsOfSpeech.AdjectiveSuperlative, PartsOfSpeech.Adverb, AbstractCollinsHeadFinder.QP };
				for (int currentConstituent = constituents.Length - 1; currentConstituent >= 0; currentConstituent--)
				{
					for (int currentTag = tags3.Length - 1; currentTag >= 0; currentTag--)
					{
						if (constituents[currentConstituent].Type.Equals(tags3[currentTag]))
						{
							return (constituents[currentConstituent].Head);
						}
					}
				}
				return (constituents[constituents.Length - 1].Head);
			}
			else
			{
                if (mHeadRules.ContainsKey(type))
				{
                    HeadRule headRule = mHeadRules[type];
					string[] tags = headRule.Tags;
					int constituentCount = constituents.Length;
					int tagCount = tags.Length;
					if (headRule.LeftToRight)
					{
						for (int currentTag = 0; currentTag < tagCount; currentTag++)
						{
							for (int currentConstituent = 0; currentConstituent < constituentCount; currentConstituent++)
							{
								if (constituents[currentConstituent].Type.Equals(tags[currentTag]))
								{
									return (constituents[currentConstituent].Head);
								}
							}
						}
						return (constituents[0].Head);
					}
					else
					{
						for (int currentTag = 0; currentTag < tagCount; currentTag++)
						{
							for (int currentConstituent = constituentCount - 1; currentConstituent >= 0; currentConstituent--)
							{
								if (constituents[currentConstituent].Type.Equals(tags[currentTag]))
								{
									return (constituents[currentConstituent].Head);
								}
							}
						}
						return (constituents[constituentCount - 1].Head);
					}
				}
			}
			return (constituents[constituents.Length - 1].Head);
		}
		
		private void ReadHeadRules(string file)
		{
#if DNF
            using (var headRulesStreamReader = new StreamReader(file, System.Text.Encoding.UTF7))
#else
            using (var stream = new FileStream(file, FileMode.OpenOrCreate))
            using (var headRulesStreamReader = new StreamReader(stream, System.Text.Encoding.UTF7))
#endif
            {
				string line = headRulesStreamReader.ReadLine();
                mHeadRules = new Dictionary<string, HeadRule>(30);
			
				while (line != null)
				{
					var tokenizer = new Util.StringTokenizer(line);
					string number = tokenizer.NextToken();
					string type = tokenizer.NextToken();
					string direction = tokenizer.NextToken();
					var tags = new string[int.Parse(number, System.Globalization.CultureInfo.InvariantCulture)];
					int currentTag = 0;
					string tag = tokenizer.NextToken();
					while (tag != null)
					{
						tags[currentTag] = tag;
						currentTag++;
						tag = tokenizer.NextToken();
					}
					mHeadRules[type] = new HeadRule((direction == "1"), tags);
					line = headRulesStreamReader.ReadLine();
				}			
			}
		}
		
		private class HeadRule
		{
			public readonly bool LeftToRight;
			public readonly string[] Tags;
			public HeadRule(bool leftToRight, string[] tags)
			{
				LeftToRight = leftToRight;
				Tags = tags;
			}
		}
	}
}
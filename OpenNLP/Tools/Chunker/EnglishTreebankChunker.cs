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

//This file is based on the EnglishTreebankChunker.java source file found in the
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Chunker
{
	/// <summary>
	/// This is a chunker based on the CONLL chunking task which uses Penn Treebank constituents as the basis for the chunks.
	/// See   http://cnts.uia.ac.be/conll2000/chunking/ for data and task definition.
	/// </summary>
	/// <author> 
	/// Tom Morton
	/// </author>
	public class EnglishTreebankChunker : MaximumEntropyChunker
	{
		/// <summary>
		/// Creates an English Treebank Chunker which uses the specified model file.
		/// </summary>
		/// <param name="modelFile">
		/// The name of the maxent model to be used.
		/// </param>
		public EnglishTreebankChunker(string modelFile) : base(new SharpEntropy.GisModel(new SharpEntropy.IO.BinaryGisModelReader(modelFile)))
		{
		}
						
		/// <summary>
		/// This method determines whether the outcome is valid for the preceding sequence.  
		/// This can be used to implement constraints on what sequences are valid.  
		/// </summary>
		/// <param name="outcome">
		/// The outcome.
		/// </param>
		/// <param name="sequence">
		/// The preceding sequence of outcome assignments. 
		/// </param>
		/// <returns>
		/// true if the outcome is valid for the sequence, false otherwise.
		/// </returns>
		protected internal override bool ValidOutcome(string outcome, Util.Sequence sequence)
		{
			if (outcome.StartsWith("I-"))
			{
				string[] tags = sequence.Outcomes.ToArray();
				int lastTagIndex = tags.Length - 1;
				if (lastTagIndex == - 1)
				{
					return (false);
				}
				else
				{
					string lastTag = tags[lastTagIndex];
					if (lastTag == "O")
					{
						return false;
					}
					if (lastTag.Substring(2) != outcome.Substring(2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}

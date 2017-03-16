using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Speechmatics.API
{
    /// <summary>
    /// Class describing a single transcription job uniquely identified by Id
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Constructor for a transcription job (from a file upload action)
        /// </summary>
        /// <param name="id">Unique job Id</param>
        /// <param name="cost">Cost (in pence) for the transcription of this job</param>
        public Job(int id, int cost)
        {
            Id = id;
            Cost = cost;
        }
        /// <summary>
        /// Unique job Id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Cost (in pence) for the transcription of this job
        /// </summary>
        public int Cost { get; private set; }
        /// <summary>
        /// Name of original Audio File that is being transcribed in this job
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Current status of this job
        /// </summary>
        /// <remarks>
        /// The values this field can take are (in order of occurence): 
        /// uploaded, recognising, aligning, diarizing, punctuating, complete, processed
        /// </remarks>
        public string Status { get; set; }

    }
}

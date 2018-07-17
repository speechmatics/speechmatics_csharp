namespace Speechmatics.API
{
    /// <summary>
    /// Response from uploading a new audio file to the API for transcription
    /// </summary>
    public class CreateJobResponse
    {
        /// <summary>
        /// Constructor from JSON response
        /// </summary>
        /// <param name="jobId">Unique Job Id</param>
        /// <param name="cost">Cost (in pence) for the transcription of this job</param>
        /// <param name="balance">User's remaining balance (in pence)</param>
        public CreateJobResponse(int jobId, int cost, int balance)
        {
            Job = new Job(jobId, cost);
        }

        /// <summary>
        /// The new job created by this action
        /// </summary>
        public Job Job { get; }
        /// <summary>
        /// User's remaining balance (in pence)
        /// </summary>
        public int Balance { get; private set; }
    }
}

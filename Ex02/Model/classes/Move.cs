namespace Ex02.Model
{
    public class Move
    {
        public int FromRow { get; }

        public int FromCol { get; }

        public int ToRow { get; }

        public int ToCol { get; }

        public bool IsCaptureMove { get; set; }

        public Move(int i_FromRow = -1, int i_FromCol = -1, int i_ToRow = -1, int i_ToCol = -1, bool i_IsCaptureMove = false)
        {
            FromRow = i_FromRow;
            FromCol = i_FromCol;
            ToRow = i_ToRow;
            ToCol = i_ToCol;
            IsCaptureMove = i_IsCaptureMove;
        }
    }
}

namespace Ex02.Model
{
    public struct Position
    {
        public int CurrentRow { get; set; }
        public int CurrentCol { get; set; }

        public Position(int i_Row, int i_Col)
        {
            CurrentRow = i_Row;
            CurrentCol = i_Col;
        }
    }
}
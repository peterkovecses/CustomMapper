namespace CustomMapper.Tests.HelperClasses
{
    internal class Dog
    {
        private int _id;
        private string _variety;


        public int Id 
        { 
            set { _id = value; }         
        }

        public string Name { get; set; }
        public string Variety
        {
            get { return _variety; }
        }
    }
}

using System;
namespace ProjetAPILinQ.Models
{
	public class Pokemon
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string[] Attack { get; set; }
		public string Image { get; set; }

        public Pokemon()
		{
		}
	}
}


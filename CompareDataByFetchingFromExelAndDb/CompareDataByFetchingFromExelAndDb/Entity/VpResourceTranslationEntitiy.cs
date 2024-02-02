using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareDataByFetchingFromExelAndDb.Entity
{
	public class VpResourceTranslationEntitiy
	{
		public long Id { get; set; }
		public long ResourceID { get; set; }
		public string CultureCode { get; set; }
		public string Value { get; set; }
		public DateTime? CreateDate { get; set; }
	}
}

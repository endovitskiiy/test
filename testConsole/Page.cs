using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsole
{
	public class Page
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Text { get; set; }
	}

	class PageContext : DbContext
	{
		public PageContext() : base("DbConnection")
		{

		}
		public DbSet<Page> Pages { get; set; }
	}

	interface IBook : IDisposable
	{
		Page GetPage(int number);
	}

	class BookStore : IBook
	{
		PageContext db;
		public BookStore()
		{
			db = new PageContext();
			db.Pages.Add(new Page { Id = 1, Number = 1, Text = "this is page1" });
			db.Pages.Add(new Page { Id = 2, Number = 2, Text = "this is page2" });
			db.Pages.Add(new Page { Id = 3, Number = 3, Text = "this is page3" });
			db.Pages.Add(new Page { Id = 4, Number = 4, Text = "this is page4" });
			db.SaveChanges();

		}
		public Page GetPage(int number)
		{
			return db.Pages.FirstOrDefault(p => p.Number == number);
		}
		public void Dispose()
		{
			db.Dispose();
		}
	}
	

	class BookStoreProxy : IBook
	{
		List<Page> _pages;
		BookStore _bookStore;
		public BookStoreProxy()
		{
			_pages = new List<Page>();
		}
		
		public Page GetPage(int number)
		{
			Page page = _pages.FirstOrDefault(x => x.Number == number);
			if (page == null)
			{
				if (_bookStore == null)
					_bookStore = new BookStore();
				page = _bookStore.GetPage(number);
				_pages.Add(page);
			}
			return page;
		}

		public void Dispose()
		{
			if(_bookStore != null)
				_bookStore.Dispose();
		}
	}
}

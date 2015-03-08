# ASP.NET MVC keynotes

## In Controller take Application User or Id

    //ApplicationDbContext dbc = new ApplicationDbContext();
    //ApplicationUser appUser = dbc.Users.FirstOrDefault(x => x.Id == this.HttpContext.User.Identity.GetUserId());
    reading.ReadenDate = DateTime.Now;
    reading.UserID = this.HttpContext.User.Identity.GetUserId();


For Recreate Database while model in Code-First approach changed. (Also you can create class based on DropCreateDatabaseIfModelChanges and add create data in Seed function) In Global.asax.cs:

    using System.Data.Entity;
    ...
    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ReadingBooksContext>());

Initializer:

    public class ReadingBooksInitializer :
                System.Data.Entity.DropCreateDatabaseIfModelChanges<ReadingBooksContext>
    {
        protected override void Seed(ReadingBooksContext context)
        {
            var books = new List<Book>() { 
                new Book() {Title="Hello world!"},
                new Book() {Title="Hello university!"}
            };
            books.ForEach(s => context.Bookss.Add(s));
            context.SaveChanges();
        }
    }

In Models:

    namespace ReadingBooks.Models
    {
        public class ReadingBooksContext : DbContext
        {
            public DbSet<Author> Authors { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<Reading> Readings { get; set; }
        }
    }

## Upload Image Fila and resize:
Controller:

    // POST: Books/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "BookID,AuthorID,Title,ReadenDate")] Book book,
                            HttpPostedFileBase file=null)
    {
        if (ModelState.IsValid)
        {
            if(file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    #region Resize image section
                    var imageFile = System.Drawing.Image.FromStream(file.InputStream);
                    int imageHeight = imageFile.Height;
                    int imageWidth = imageFile.Width;
                    int maxHeight = 120;
                    int maxWidth = 160;

                    imageHeight = (imageHeight * maxWidth) / imageWidth;
                    imageWidth = maxWidth;

                    if (imageHeight > maxHeight)
                    {
                        imageWidth = (imageWidth * maxHeight) / imageHeight;
                        imageHeight = maxHeight;
                    }

                    var bitmapFile = new Bitmap(imageFile, imageWidth, imageHeight);
                    var stream = new System.IO.MemoryStream();

                    bitmapFile.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    stream.Position = 0;

                    byte[] data = new byte[stream.Length + 1];
                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    #endregion
                    book.Cover = data;
                }
            }
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(book);
    }

    <div class="form-group">
                @Html.LabelFor(model => model.Cover, htmlAttributes: new { @class = "control-label col-md-2" })
                <div class="col-md-10">
                    @{ Html.EnableClientValidation(false); }
                    @Html.EditorFor(model => model.Cover, new { htmlAttributes = new { @class = "form-control" } })
                    @Html.ValidationMessageFor(model => model.Cover, "", new { @class = "text-danger" })
                    @{ Html.EnableClientValidation(true); }
                </div>
            </div>

## Models and Context

    public class Author
    {
        public int AuthorID { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Author Name")]
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }
        [DisplayName("Author Surname")]
        [Required(ErrorMessage = "Please enter a Surname.")]
        public string Surname { get; set; }

        [DisplayName("Author Books")]
        virtual public List<Book> books { get; set; }
    }

    public class Book
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Title of the book")]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Readen Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime ReadenDate { get; set; }

        [DisplayName("Cover")]
        public byte[] Cover { get; set; }
    }
    public class BookReaderContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }

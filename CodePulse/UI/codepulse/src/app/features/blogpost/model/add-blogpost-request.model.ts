export interface AddBlogPostRequest {
  title: string,
  shortDescription: string,
  content: string,
  featuredImageUrl: string,
  urlHandle: string,
  publishedDate: Date,
  author: string,
  isVisible: boolean
}


/*
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string  Content{ get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }

*/

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogPostService } from '../../blogpost/service/blogpost.service';
import { Observable, Subscription } from 'rxjs';
import { BlogPost } from '../../blogpost/model/blogpost.model';

@Component({
  selector: 'app-blog-details',
  templateUrl: './blog-details.component.html',
  styleUrl: './blog-details.component.css'
})
export class BlogDetailsComponent implements OnInit{

  url: string | null = null;
  blog$?: Observable<BlogPost>;
  constructor(private route: ActivatedRoute,
              private blogService: BlogPostService
  ){

  }
  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        this.url = params.get('url');
      }
    })

    if (this.url){
      this.blog$ = this.blogService.getBlogByUrl(this.url);
    }
  }




}

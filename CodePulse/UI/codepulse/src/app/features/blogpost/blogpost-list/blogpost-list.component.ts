import { Component, OnInit } from '@angular/core';
import { BlogPostService } from '../service/blogpost.service';
import { BlogPost } from '../model/blogpost.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrl: './blogpost-list.component.css'
})
export class BlogpostListComponent implements OnInit {

  //BlogPosts?: BlogPost[];

  BlogPost$?: Observable<BlogPost[]>;

  constructor(public blogService: BlogPostService){

  }
  ngOnInit(): void {
    this.BlogPost$ = this.blogService.getAllPost();
    console.log(this.BlogPost$);
  }



}

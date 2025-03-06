import { Component, OnInit } from '@angular/core';
import { BlogPostService } from '../../blogpost/service/blogpost.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blogpost/model/blogpost.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  blogs$?: Observable<BlogPost[]>
  constructor(private blogPostService: BlogPostService){}


  ngOnInit(): void {
    this.blogs$ = this.blogPostService.getAllPost();

    console.log(this.blogs$.subscribe({
      next: (response) => {
        console.log(response)
      }
    }));
  }




}

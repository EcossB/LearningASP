import { Component, OnDestroy } from '@angular/core';
import { AddBlogPostRequest } from '../model/add-blogpost-request.model';
import { BlogPostService } from '../service/blogpost.service';
import { Router, RouterLink } from '@angular/router';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.css'
})
export class AddBlogpostComponent implements OnDestroy{
  blogpostRequest: AddBlogPostRequest;
  blogpostSubscription?: Subscription;

  constructor(private BlogService: BlogPostService,
              private router: Router
  ){
    this.blogpostRequest = {
      title: '',
      shortDescription: '',
      content: '',
      featuredImageUrl: '',
      urlHandle: '',
      publishedDate: new Date(),
      author: '',
      isVisible: true
    };
  }

  ngOnDestroy(): void {
    this.blogpostSubscription?.unsubscribe();
  }

  onSubmitForm(): void {
    this.blogpostSubscription = this.BlogService.addBlogPost(this.blogpostRequest).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.log(error);
        console.log(this.blogpostRequest);
      },
      complete: () => {
        this.router.navigateByUrl('/admin/blogpost');
      }
    })
  }

}

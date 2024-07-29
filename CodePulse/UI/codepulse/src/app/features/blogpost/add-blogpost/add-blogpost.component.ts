import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPostRequest } from '../model/add-blogpost-request.model';
import { BlogPostService } from '../service/blogpost.service';
import { Router, RouterLink } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.css'
})
export class AddBlogpostComponent implements OnDestroy, OnInit{
  blogpostRequest: AddBlogPostRequest;
  blogpostSubscription?: Subscription;
  Categories$?: Observable<Category[]>;

  constructor(private BlogService: BlogPostService,
              private CategoryService: CategoryService,
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
      isVisible: true,
      category: []
    };
  }


  ngOnInit(): void {
    this.Categories$ = this.CategoryService.getAllCategories();
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

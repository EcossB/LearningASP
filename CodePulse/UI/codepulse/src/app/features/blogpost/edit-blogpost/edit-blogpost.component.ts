import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPost } from '../model/blogpost.model';
import { BlogPostService } from '../service/blogpost.service';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { UpdateCategoryRequest } from '../../category/models/update-category-request.model';
import { EditBlogPostRequest } from '../model/edit-blogpost-request.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.css'
})
export class EditBlogpostComponent implements OnInit, OnDestroy {


  id: string | null = null;
  idsubscription$?: Subscription;
  updateBlogSubscription?: Subscription;
  getBlogSubscription?: Subscription;
  BlogPost?: BlogPost;
  Categories$?: Observable<Category[]>;
  selectedCategories?: string[];

  /**
   * @route to get the id from the url.
   */
  constructor(private route: ActivatedRoute,
              private BlogPostService: BlogPostService,
              private categoryService: CategoryService,
              private router: Router
  ) {

  }

  ngOnInit(): void {
    this.idsubscription$ = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id')

        if (this.id){
          this.getBlogSubscription = this.BlogPostService.getPostById(this.id).subscribe({
            next: (response) => {
              this.BlogPost = response;
              this.selectedCategories = this.BlogPost.category.map(x => x.id); //mapping the id of the categorys in the response.
              this.Categories$ = this.categoryService.getAllCategories();
              console.log(this.BlogPost)
            }
          })
        }

      }
    })
  }

  onSubmitForm() {

    if(this.BlogPost && this.id){

      const updateBlog : EditBlogPostRequest = {
        title: this.BlogPost.title ?? '',
        shortDescription: this.BlogPost.shortDescription ?? '',
        content: this.BlogPost.content ?? '',
        featuredImageUrl: this.BlogPost.featuredImageUrl ?? '',
        urlHandle: this.BlogPost.urlHandle ?? '',
        publishedDate: this.BlogPost.publishedDate,
        author: this.BlogPost.author ?? '',
        isVisible: this.BlogPost.isVisible ?? false,
        category: this.selectedCategories ?? []
      }


      this.updateBlogSubscription = this.BlogPostService.updatePost(this.id, updateBlog).subscribe({
        next: () => {
          console.log(updateBlog);
          this.router.navigateByUrl('/admin/blogpost');
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.idsubscription$?.unsubscribe();
    this.updateBlogSubscription?.unsubscribe();
    this.getBlogSubscription?.unsubscribe();
  }


}

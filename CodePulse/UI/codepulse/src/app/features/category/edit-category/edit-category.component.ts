import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrl: './edit-category.component.css'
})
export class EditCategoryComponent implements OnInit, OnDestroy{


  id: string | null = null; // here we store the id pass it through params
  paramSubscription?: Subscription;
  editSubscription?: Subscription;
  deleteSubscription?: Subscription;
  category?: Category;

  constructor(private route: ActivatedRoute,
              private categoryService: CategoryService,
              private router: Router
  ){}



  ngOnDestroy(): void {
    this.paramSubscription?.unsubscribe();
    this.editSubscription?.unsubscribe();
    this.deleteSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.paramSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id){
          this.categoryService.getCategoryById(this.id).subscribe({
            next: (response) => {
              this.category = response;
            }
          })
        }
      }
    })
  }

  onFormSubmit(category: Category) {

    const updateCategory: UpdateCategoryRequest = {
      name: category.name ?? '',
      urlHandle: category.urlHandle ?? ''
    };

    if(this.id){
      this.deleteSubscription = this.editSubscription = this.categoryService.updateCategory(this.id, updateCategory).subscribe({
        next: (response) => {
          console.log(response);
        },
        complete: () =>{
          this.router.navigateByUrl('/admin/categories');
        }
      })
    }
   }

   onDeleteCategory() {

    if(this.id){
      this.categoryService.deleteCategory(this.id).subscribe({
        next: () =>{
            this.router.navigateByUrl('/admin/categories');
        }
      })
    }
  }


}

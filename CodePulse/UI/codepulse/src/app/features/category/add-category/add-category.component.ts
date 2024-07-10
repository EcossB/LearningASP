import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent implements OnDestroy{

  model: AddCategoryRequest;
  private addCategorySubscription?: Subscription;

  constructor(private categoryService: CategoryService){

    /**
     * inicializando el modelo.
     */
    this.model = {
      Name: '',
      UrlHandle: ''
    };

  }

  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }


  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model).subscribe({
      next: ((data) => {
          console.log('Data send.')
      }),
      error: ((error) => {
        console.log("an error ocurried.")
      })
    });
  }



}

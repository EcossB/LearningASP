import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent implements OnDestroy{

  model: AddCategoryRequest;
  private addCategorySubscription?: Subscription;

  constructor(private categoryService: CategoryService,
              private router: Router
  ){

    /**
     * inicializando el modelo.
     */
    this.model = {
      Name: '',
      UrlHandle: ''
    };

  }

  /**
   * Es bueno desuscribirse de las suscripciones hechas
   * para que no haya un laqueo de memoria y tambien la optimizacion es mejor.
   */
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
      }),
      complete: () => {
        this.router.navigateByUrl('/admin/categories');
      }
    });
  }



}

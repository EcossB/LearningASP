import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit, OnDestroy{

  // categoryList!: Category[];

  /**
   * using an async pipe. Async pipe is a feature that subscribe automatically to an observable.
   */

  categories$?: Observable<Category[]>;

  private addCategorySubscription?: Subscription;

  constructor(private categoryService: CategoryService){

  }

  loadCategories() {
    this.categories$ = this.categoryService.getAllCategories();
  }

  ngOnInit(): void {
    this.loadCategories();
  }
  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }



}

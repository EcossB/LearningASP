import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit, OnDestroy{

  categoryList!: Category[];
  private addCategorySubscription?: Subscription;

  constructor(private categoryService: CategoryService){

  }

  loadCategories() {
    this.addCategorySubscription = this.categoryService.getAllCategories().subscribe({
      next: (response) => {
        this.categoryList = response;
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        console.log(this.categoryList);
      }
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }
  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }



}

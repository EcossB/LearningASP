import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { BlogpostListComponent } from './features/blogpost/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './features/blogpost/add-blogpost/add-blogpost.component';
import { EditBlogpostComponent } from './features/blogpost/edit-blogpost/edit-blogpost.component';

const routes: Routes = [
  {path: 'admin/categories', title: 'Category List', component: CategoryListComponent},
  {path: 'admin/categories/add', title: 'Add Category', component:AddCategoryComponent},
  {path: 'admin/categories/:id', title: 'Edit Category', component:EditCategoryComponent},
  {path: 'admin/blogpost', title: 'BlogPost', component:BlogpostListComponent},
  {path: 'admin/blogpost/add', title: 'Add BlogPost', component:AddBlogpostComponent},
  {path: 'admin/blogpost/:id', title: 'Edit BlogPost', component:EditBlogpostComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

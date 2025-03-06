import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { BlogpostListComponent } from './features/blogpost/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './features/blogpost/add-blogpost/add-blogpost.component';
import { EditBlogpostComponent } from './features/blogpost/edit-blogpost/edit-blogpost.component';
import { HomeComponent } from './features/public/home/home.component';
import { BlogDetailsComponent } from './features/public/blog-details/blog-details.component';
import { LoginComponent } from './features/auth/login/login.component';
import { authGuard } from './features/auth/guards/auth.guard';

const routes: Routes = [
  {path: 'admin/categories', title: 'Category List', component: CategoryListComponent, canActivate: [authGuard]},
  {path: 'admin/categories/add', title: 'Add Category', component:AddCategoryComponent, canActivate: [authGuard]},
  {path: 'admin/categories/:id', title: 'Edit Category', component:EditCategoryComponent, canActivate: [authGuard]},
  {path: 'admin/blogpost', title: 'BlogPost', component:BlogpostListComponent, canActivate: [authGuard]},
  {path: 'admin/blogpost/add', title: 'Add BlogPost', component:AddBlogpostComponent, canActivate: [authGuard]},
  {path: 'admin/blogpost/:id', title: 'Edit BlogPost', component:EditBlogpostComponent, canActivate: [authGuard]},
  {path: '', title: 'Home', component:HomeComponent},
  {path: 'blog/:url', title:'Blog Detail', component:BlogDetailsComponent},
  {path: 'login', title:'Login', component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

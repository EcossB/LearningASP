import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { environment } from '../../../../environments/environment.development';
import { UpdateCategoryRequest } from '../models/update-category-request.model';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {



  constructor(private http: HttpClient,
              private coolies: CookieService
  ) {}

   urlApi: string = `${environment.apiBaseUrl}/api/Categories`;


   addCategory(model: AddCategoryRequest): Observable<void>{
     return this.http.post<void>(`${this.urlApi}?addAuth=true`,model);
   }

   getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.urlApi);
   }

   getCategoryById(id: string): Observable<Category>{
      return this.http.get<Category>(`${this.urlApi}/${id}`);
   }

   updateCategory(id:string, updateCategory: UpdateCategoryRequest): Observable<Category>{
      return this.http.put<Category>(`${this.urlApi}/${id}?addAuth=true`, updateCategory);
   }

   deleteCategory(id: string ): Observable<void> {
    return this.http.delete<void>(`${this.urlApi}/${id}?addAuth=true`);
   }

}

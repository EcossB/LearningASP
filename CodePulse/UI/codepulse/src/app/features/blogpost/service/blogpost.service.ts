import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { AddBlogPostRequest } from '../model/add-blogpost-request.model';
import { Observable } from 'rxjs';
import { BlogPost } from '../model/blogpost.model';
import { EditBlogPostRequest } from '../model/edit-blogpost-request.model';

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {

  constructor(private http: HttpClient) { }

  urlApi: string = `${environment.apiBaseUrl}/api/BlogPost`;

  addBlogPost(blogpost: AddBlogPostRequest): Observable<BlogPost>{
    return this.http.post<BlogPost>(`${this.urlApi}?addAuth=true`, blogpost);
  }

  getAllPost(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(this.urlApi);
  }

  getPostById(id: string): Observable<BlogPost>{
    return this.http.get<BlogPost>(`${this.urlApi}/${id}`)
  }

  updatePost(id: string, updateCategory: EditBlogPostRequest): Observable<BlogPost>{
    return this.http.put<BlogPost>(`${this.urlApi}/${id}?addAuth=true`, updateCategory);
  }

  deletePostById(id:string): Observable<void>{
    return this.http.delete<void>(`${this.urlApi}/${id}?addAuth=true`);
  }

  getBlogByUrl(url:string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.urlApi}/${url}`);
  }


}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { AddBlogPostRequest } from '../model/add-blogpost-request.model';
import { Observable } from 'rxjs';
import { BlogPost } from '../model/blogpost.model';

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {

  constructor(private http: HttpClient) { }

  urlApi: string = `${environment.apiBaseUrl}/api/BlogPost`;

  addBlogPost(blogpost: AddBlogPostRequest): Observable<BlogPost>{
    return this.http.post<BlogPost>(this.urlApi, blogpost);
  }

  getAllPost(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(this.urlApi);
  }


}

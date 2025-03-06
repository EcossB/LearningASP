import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { BlogImage } from '../../models/blog-image-model';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) {}

  urlApi = `${environment.apiBaseUrl}/api/Images`;

  selectedImage: BehaviorSubject<BlogImage> = new BehaviorSubject<BlogImage>({
    id: '',
    fileName: '',
    fileExtension: '',
    title: '',
    url: ''
  });

  uploadImage(file: File, fileName: string, title: string): Observable<BlogImage>{

    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);


    return this.http.post<BlogImage>(this.urlApi, formData);
  }

  getAllImage(): Observable<BlogImage[]>{
    return this.http.get<BlogImage[]>(this.urlApi);
  }

  selectImage(image: BlogImage): void {
    this.selectedImage.next(image);
  }

  onSelectImage(): Observable<BlogImage> {
    return  this.selectedImage.asObservable();
  }

}

import { Component, OnInit, ViewChild } from '@angular/core';
import { ImageService } from './image.service';
import { Observable } from 'rxjs';
import { BlogImage } from '../../models/blog-image-model';
import { Form, NgForm } from '@angular/forms';

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrl: './image-selector.component.css'
})
export class ImageSelectorComponent implements OnInit{

  private file?: File;

  fileName : string = '';
  title : string = '';
  images$?: Observable<BlogImage[]>;
 @ViewChild('ngForm',{static: false}) imageUploadForm?: NgForm;

  constructor(private imageService: ImageService) {

  }
  ngOnInit(): void {
    this.getAllImage();
  }

  onFileUploadChange(event: Event): void{
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0]; // this retreive the first entry of the htmlInputElement.
  }

  uploadImage(): void {
    if(this.file && this.fileName !== '' && this.title !== null){
      this.imageService.uploadImage(this.file, this.fileName, this.title).subscribe({
        next: (): void => {
          this.imageUploadForm?.resetForm();
          this.getAllImage();
        }
      })
    }

  }

  selectImage(image: BlogImage) {
    this.imageService.selectImage(image);
  }

  private getAllImage(): void{
    this.images$ = this.imageService.getAllImage(); //asignando el observable a una variable para hacerle un async pipe
  }

}

import { Component, AfterViewInit, OnInit } from '@angular/core';

import { s3 } from 'fine-uploader/lib/core/s3';

@Component({
  selector: 'app-uploads',
  templateUrl: './uploads.component.html',
  styleUrls: ['./uploads.component.css']
})
export class UploadsComponent implements AfterViewInit {
  bucketName = 'tvdiassampleupload';
  accessKey = 'AKIAVXUMNUAFSFFCVDMT';
  endpoint = 'https://' + this.bucketName + '.s3.amazonaws.com/';

  uploader: any;

  ngAfterViewInit() {
    let instance = this;
    this.uploader = new s3.FineUploaderBasic({
      button: document.getElementById('upload_image'),
      debug: false,
      autoUpload: true,
      multiple: true,
      validation: {
        allowedExtensions: ['jpeg', 'jpg', 'png', 'gif', 'svg'],
        sizeLimit: 5120000 // 50 kB = 50 * 1024 bytes
      },
      request: {
        endpoint: instance.endpoint,
        accessKey: instance.accessKey,
        params: { 'Cache-Control': 'private, max-age=31536000, must-revalidate' }
      },
      signature: {
        endpoint: 'https://localhost:44314/api/uploads/authorize',
        version: 4
      },
      cors: {
        expected: true,
        sendCredentials: true
      },
      objectProperties: {
        region: 'eu-west-1',
      },
      callbacks: {
        onSubmit: function (id, fileName) {
          console.log('selected file:', fileName);
        },
        onComplete: function (id, name, responseJSON, maybeXhr) {
          if (responseJSON.success) {
            console.log('upload complete', name);
            console.log('uploaded image url', instance.endpoint + this.getKey(id));
          }
        }
      }
    });
  }
}

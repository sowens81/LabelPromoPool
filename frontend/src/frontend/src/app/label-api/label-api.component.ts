import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-label-api',
  templateUrl: './label-api.component.html',
  styleUrls: ['./label-api.component.css']
})
export class LabelApiComponent implements OnInit {

  labels: string;

  constructor(private api: ApiService) { }

  ngOnInit() {
  }

  pingApi() {
    this.api.ping$().subscribe(
      res => this.labels = res
    );
  }
}

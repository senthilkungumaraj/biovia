import { Component, OnInit, ViewChildren, QueryList, AfterViewInit, NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../api.service';
import { Study } from '../study';
import { Project } from '../project';

@Component({
  selector: 'app-studies',
  templateUrl: './studies.component.html',
  styleUrls: ['./studies.component.scss']
})
@NgModule({
  imports: [
    MatPaginator
  ],
  exports: [
    MatPaginator
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class StudiesComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['name', 'description'];
  data: MatTableDataSource<Study>;
  isLoadingResults = true;
  projectData: Project;

  constructor(private api: ApiService, private route: ActivatedRoute) { }

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  ngOnInit() {
    this.api.getStudies(this.route.snapshot.params['id'])
      .subscribe(res => {
        this.data = new MatTableDataSource<Study>(res);
        console.log(this.data);
        this.isLoadingResults = false;
      }, err => {
        console.log(err);
        this.isLoadingResults = false;
      });

    this.api.getProject(this.route.snapshot.params['id'])
      .subscribe(res => {
        this.projectData = res;
        console.log(this.projectData);
        this.isLoadingResults = false;
      }, err => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }
  ngAfterViewInit() {
    this.data.paginator = this.paginator.toArray()[0];
    this.data.sort = this.sort.toArray()[0];
    this.data.sort.sortChange.subscribe(this.sortChanged);
  }

  
  public sortChanged(event: any) {
    console.log('Sort Changed');
  }

}

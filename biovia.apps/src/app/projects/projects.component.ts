import { Component, OnInit, ViewChildren, AfterViewInit, QueryList } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiService } from '../api.service';
import { Project } from '../project';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['name', 'description'];
  data: MatTableDataSource<Project>;
  isLoadingResults = true;

  constructor(private api: ApiService) { }

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  ngOnInit() {
    this.api.getProjects()
      .subscribe(res => {
        this.data = new MatTableDataSource<Project>(res);
        console.log(this.data);
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

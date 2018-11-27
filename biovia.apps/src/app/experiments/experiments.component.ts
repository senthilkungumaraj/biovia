import { Component, OnInit, ViewChildren, QueryList, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../api.service';
import { Experiment } from '../experiment';
import { Study } from '../study';

@Component({
  selector: 'app-experiments',
  templateUrl: './experiments.component.html',
  styleUrls: ['./experiments.component.scss']
})
export class ExperimentsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['index', 'name', 'creationDate'];
  data: MatTableDataSource<Experiment>;
  isLoadingResults = true;
  projectId: string;
  studyId: string;
  studyData: Study;

  constructor(private api: ApiService, private route: ActivatedRoute) { }

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();
  @ViewChildren(MatSort) sort = new QueryList<MatSort>();
  ngOnInit() {
    this.projectId = this.route.snapshot.params['pid'];
    this.studyId = this.route.snapshot.params['sid'];
    this.reloadData();
  }
  ngAfterViewInit() {
    this.data.paginator = this.paginator.toArray()[0];
    this.data.sort = this.sort.toArray()[0];
    this.data.sort.sortChange.subscribe(this.sortChanged);
  }
  sortChanged = (event: any) => {
    this.isLoadingResults = true;
    this.api.updateExperimentOrder(this.projectId, this.studyId, event.active, event.direction)
      .subscribe(res => {
          // this.reloadData();
          this.isLoadingResults = false;
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        });
  }

  reloadData = () => {
    this.api.getExperiments(this.route.snapshot.params['pid'], this.route.snapshot.params['sid'])
    .subscribe(res => {
      this.data = new MatTableDataSource<Experiment>(res);
      console.log(this.data);
      this.isLoadingResults = false;
    }, err => {
      console.log(err);
      this.isLoadingResults = false;
    });
    this.api.getStudy(this.route.snapshot.params['pid'], this.route.snapshot.params['sid'])
    .subscribe(res => {
      this.studyData = res;
      console.log(this.studyData);
      this.isLoadingResults = false;
    }, err => {
      console.log(err);
      this.isLoadingResults = false;
    });
  }

}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../api.service';
import { Project } from '../project';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.scss']
})
export class ProjectDetailComponent implements OnInit {

  project: Project = { proj_id: '', name: '', description: '' };
  isLoadingResults = true;

  constructor(private route: ActivatedRoute, private api: ApiService, private router: Router) { }

  ngOnInit() {
    console.log(this.route.snapshot.params['id']);
    this.getProjectDetails(this.route.snapshot.params['id']);
  }

  getProjectDetails(id) {
    this.api.getProject(id)
      .subscribe(data => {
        this.project = data;
        console.log(this.project);
        this.isLoadingResults = false;
      });
  }

  deleteProject(id) {
    this.isLoadingResults = true;
    this.api.deleteProject(id)
      .subscribe(res => {
          this.isLoadingResults = false;
          this.router.navigate(['/projects']);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        }
      );
  }

}

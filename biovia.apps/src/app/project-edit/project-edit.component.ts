import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../api.service';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.scss']
})
export class ProjectEditComponent implements OnInit {

  projectForm: FormGroup;
  _id:string='';
  proj_name:string='';
  proj_desc:string='';
  prod_price:number=null;
  isLoadingResults = false;

  constructor(private router: Router, private route: ActivatedRoute, private api: ApiService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getProject(this.route.snapshot.params['id']);
    this.projectForm = this.formBuilder.group({
      'proj_name' : [null, Validators.required],
      'proj_desc' : [null, Validators.required],
      'prod_price' : [null, Validators.required]
    });
  }

  getProject(id) {
    this.api.getProject(id).subscribe(data => {
      this._id = data.proj_id;
      this.projectForm.setValue({
        proj_name: data.name,
        proj_desc: data.description,
      });
    });
  }

  onFormSubmit(form:NgForm) {
    this.isLoadingResults = true;
    this.api.updateProject(this._id, form)
      .subscribe(res => {
          let id = res['_id'];
          this.isLoadingResults = false;
          this.router.navigate(['/project-details', id]);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        }
      );
  }

  projectDetails() {
    this.router.navigate(['/project-details', this._id]);
  }

}

import React, { Component } from "react";
import Actions from "../../common/reuseable/Actions";
import IssuedBoxScanTagActions from "../../common/reuseable/IssuedBoxScanTagActions";
import Table from "../../common/ui-controls/Table";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import Stack from "@material-ui/core/Stack";
import Button from "@material-ui/core/Button";
import Snackbar from "@material-ui/core/Snackbar";
import FieldSet from "../../common/reuseable/FieldSet";
import DialogComponent from "../../common/ui-controls/DialogComponent";
import Menu from "@material-ui/core/Menu";
import MenuItem from "@material-ui/core/MenuItem";
import _, { clone } from "lodash";
import { ContactPageOutlined } from "@material-ui/icons";
import { Link } from "@material-ui/core";
import config from "../../../config.json";

class IssuedBoxDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
      contextMenuOpen: false,
      saveAction: ""
    };

    this.contextMenuOptions = ["Delete", "Edit Serial Number", "Edit PIN"];
  }

  render() {
    return (
      <React.Fragment>
        {this.props.dialogOpen && (
          <DialogComponent
            title="SUMMARY"
            open={this.props.dialogOpen}
            content={this.props.onRenderDialog()}
            actions={[
              { action: "continue", elementType: "dialog", text: "OK" },
              {
                action: "cancel",
                elementType: "dialog",
                text: "Cancel"
              }
            ]}
            onDialogClose={this.props.onHandleDialogClose}
          ></DialogComponent>
        )}
        <Snackbar
          open={this.props.alertOpen}
          onClose={e =>
            this.props.onHandleDialogClose.call(this, {
              target: { action: "close", elementType: "alert" }
            })
          }
          anchorOrigin={{ vertical: "bottom", horizontal: "left" }}
          autoHideDuration={8000}
        >
          <Alert
            severity="error"
            onClose={e =>
              this.props.handleDialogClose.call(this, {
                target: { action: "close", elementType: "alert" }
              })
            }
          >
            <AlertTitle>Error</AlertTitle>
            <strong>{this.props.content}</strong>
          </Alert>
        </Snackbar>

        <div className="content-area-main">
          <div className="content-title">
            <h4>{this.props.title}</h4>
          </div>
          <div className="row top-buffer" style={{ marginLeft: 3 }}></div>

          <div className="row">
            <div className="col-md-4">
              <FieldSet
                inputValue={this.props.receivedBoxValue}
                onTextChange={this.props.onTextChange}
                onButtonClick={this.props.onButtonClick}
                form="issuedBox"
                buttonCaption="Create Box "
                labelCaption="Recieved Box"
                buttonCaptionSecond="Start Assignment"
                showSecondBtn={true}
                disableSecondBtn={this.props.disableSecondBtn}
                disableFirstBtn={this.props.disableFirstBtn}
              ></FieldSet>
            </div>
          </div>
          {this.props.showScanTagActionBar && (
            <div className="actions-import-box">
              <IssuedBoxScanTagActions
                disableVerifyScanTag={
                  this.props.issuedBox.tags.length === config.boxCount
                }
                disableAssignTags={
                  this.props.issuedBox.tags.length < config.boxCount
                }
                onLinkClick={this.props.onLinkClick}
                tagsCount={this.props.issuedBox.tags.length}
              ></IssuedBoxScanTagActions>
            </div>
          )}

          <div className="row top-buffer">
            <div className="list-container">
              {this.props.displayTable && (
                <Table
                  columns={this.props.columns}
                  data={this.props.issuedBox.tags}
                  searchVisible={false}
                ></Table>
              )}
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

export default IssuedBoxDetails;

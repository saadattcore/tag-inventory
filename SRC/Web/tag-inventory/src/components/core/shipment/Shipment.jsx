import React, { Component } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import {
  addUpdateShipment,
  getShipment
} from "../../../actions/shipmentActions";
import ShipmentDetail from "../shipment/ShipmentDetail";
import moment from "moment";
import { Link } from "react-router-dom";
import Actions from "../../common/reuseable/Actions";
import { renderAlert } from "../../common/ui-controls/withAlert";
import _ from "lodash";

class Shipment extends Component {
  constructor(props) {
    super(props);
    this.shipmentID = this.props.match.params.id
      ? this.props.match.params.id
      : -1;

    this.currentDate = this.formatDate(new Date());

    this.state = {
      shipment: {
        shipmentID: -1,
        purchaseOrder: "",
        shipmentName: "",
        orderDate: this.currentDate,
        shipmentDate: this.currentDate,
        deliveryDate: this.currentDate,
        status: "",
        statusID: 1
      },
      errors: {
        purchaseOrder: "Purchase order is required",
        shipmentName: "Shipment name is required"
      },
      isFormValid: this.shipmentID !== -1,
      showAlert: null
    };

    if (this.shipmentID !== -1) {
      delete this.state.errors["purchaseOrder"];
      delete this.state.errors["shipmentName"];
    }
  }

  formatDate(dateObject) {
    if (!dateObject) return null;

    return moment(dateObject).format("MM-DD-YYYY");
  }

  validateProperty(name, value) {
    if (value && value.toString() === "Invalid Date") return "Invalid Date";

    switch (name) {
      case "purchaseOrder":
        return !value.trim() ? "Purchase order is required" : null;
      case "orderDate":
        return !value ? "Order date is required" : null;
      case "shipmentDate":
        return !value ? "Shipment date is required" : null;
      case "deliveryDate":
        return !value ? "Delivery date is required" : null;
      case "shipmentName":
        return !value.trim() ? "Shipment name is required" : null;
    }
  }

  componentDidMount() {
    if (this.shipmentID !== -1) {
      const url = `${process.env.REACT_APP_API_URL}shipment/${this.shipmentID}`;
      this.props.getShipmentAction(url).then(response => {
        const cloneState = { ...this.state };
        cloneState.shipment = { ...response };

        cloneState.shipment.shipmentDate = response.shipmentDate
          ? response.shipmentDate
          : null;

        if (!response.deliveryDate) {
          cloneState.shipment.deliveryDate = null;
          cloneState.errors["deliveryDate"] = "Delivery date is required";
          cloneState.isFormValid = false;
        }

        this.setState(cloneState);
      });
    }
  }

  handleChange({ target }) {
    let { name, value } = target;
    const state = { ...this.state };
    state.shipment[name] = value;
    const error = this.validateProperty(name, value);

    if (error) {
      state.errors[name] = error;
      state.isFormValid = false;
    } else {
      delete state.errors[name];
    }
    if (Object.keys(state.errors).length === 0) {
      state.isFormValid = true;
    }
    this.setState(state);
  }

  handleClick(e) {
    e.target.e.preventDefault();

    switch (e.target.action) {
      case "back":
        this.props.history.push("/shipment");
        break;
      case "saveclose":
      case "save":
        const url = `${process.env.REACT_APP_API_URL}shipment/`;
        console.log(e);
        this.props
          .addUpdateShipmentAction(url, this.state.shipment)
          .then(response => {
            if (e.target.action === "saveclose") {
              this.props.history.push("/shipment");
            } else {
              const stateClone = { ...this.state };

              stateClone.showAlert = renderAlert.bind(
                null,
                "info",
                "Shipment have been created sucessfully",
                true,
                e => {
                  stateClone.showAlert = null;
                  this.setState(stateClone);
                }
              );

              this.setState(stateClone);
            }
          })
          .catch(error => {
            console.log(error);
            //const stateClone = { ...this.state };
            //stateClone.displayAlert = true;
            //stateClone.alert(error.data.Message);
          });
        break;
    }
  }

  render() {
    return (
      <form>
        <div className="actions">
          <Actions
            isFormValid={this.state.isFormValid}
            onLinkClick={this.handleClick.bind(this)}
          ></Actions>
        </div>
        <div className="title-summary-wrapper">
          <div className="content-title">
            <h3>New Shipment</h3>
          </div>
        </div>

        {this.state.showAlert && this.state.showAlert()}

        <div className="content-area">
          <div className="row">
            <div className="col-md-5">
              <ShipmentDetail
                shipment={this.state.shipment}
                errors={this.state.errors}
                readOnly={false}
                onChangeValue={this.handleChange.bind(this)}
              ></ShipmentDetail>
            </div>
          </div>
        </div>
      </form>
    );
  }
}

Shipment.propTypes = {
  shipment: PropTypes.object.isRequired,
  addUpdateShipment: PropTypes.func.isRequired,
  getShipment: PropTypes.func.isRequired
};

const mapStateToProps = state => {
  return {
    shipment: state.shipment.shipment
  };
};

const mapDispatchToProps = disptach => ({
  addUpdateShipmentAction: (url, shipment) =>
    disptach(addUpdateShipment(url, shipment)),
  getShipmentAction: url => disptach(getShipment(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Shipment);

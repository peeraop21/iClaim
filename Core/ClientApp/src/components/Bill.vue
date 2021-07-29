<template>
    <dev class="container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <div class="stepper-wrapper">
                    <div class="stepper-item completed">
                        <div class="step-counter"><ion-icon name="receipt-outline" style="font-size: 20px; color: white"></ion-icon></div>
                        <p style="color: #5c2e91">สร้างคำร้อง</p>
                    </div>
                    <div class="stepper-item">
                        <div class="step-counter"><ion-icon name="card-outline" style="font-size: 20px; color: white"></ion-icon></div>
                        <p style="color: #bbb">บัญชีรับเงิน</p>
                    </div>
                    <div class="stepper-item">
                        <div class="step-counter"><ion-icon name="send-outline" style="font-size: 20px; color: white"></ion-icon></div>
                        <p style="color: #bbb">ส่งคำร้อง</p>
                    </div>
                </div>
                <h2 id="header2">เอกสารประกอบคำร้อง</h2>
                <br>
                <div class="container">
                    <form align="left">
                        <label for="fname">อาการบาดเจ็บ</label>
                        <input type="text" id="fname" name="fname">
                    </form>
                    <br>
                    <div align="left" style="width: 92%;">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่ารักษาพยาบาลเบื้องต้น</label><br>
                    </div>
                    <div align="left" style="border-radius: 10px; border: 1px solid #cccccc; padding: 20px 20px 15px 20px;">
                        <p>สำเนาบัตรประจำตัวประชาชน</p>
                        <form action="/action_page.php">
                            <input type="file" id="myFile" name="filename">
                            <!--<input type="submit"> -->
                        </form>
                    </div>
                    <br>
                    <div align="left" style="border-radius: 10px; border: 1px solid #cccccc; padding: 20px 20px 15px 20px;">
                        <form action="/action_page.php">
                            <div v-for="(input, index) in bills" :key="`Bill-${index}`" class="input wrapper flex items-center">
                                <div v-if="!image">
                                    <p>ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                                    <input type="file" @change="onFileChange">
                                </div>
                                <div v-else>
                                    <img :src="image" />
                                    <button @click="removeImage">Remove image</button>
                                </div>
                                <br>
                                <label>โรงพยาบาล</label>
                                <input v-model="input.hospital" type="text" class="h-10 rounded-lg outline-none p-1" placeholder="" style="margin-top: -7px; margin-bottom: 10px;"/>
                                <label>เลขที่ใบเสร็จ</label>
                                <input v-model="input.billNo" type="text" class="h-10 rounded-lg outline-none p-1" placeholder="" style="margin-top: -7px; margin-bottom: 10px;"/>
                                <label>จำนวนเงิน</label>
                                <input v-model="input.money" type="text" class="h-10 rounded-lg outline-none p-1" placeholder="" style="margin-top: -7px; margin-bottom: 10px;"/>
                                <label>เข้ารักษาวันที่</label>
                                <input v-model="input.hospitalized_date" type="text" class="h-10 rounded-lg outline-none p-1" placeholder="" style="margin-top: -7px; margin-bottom: 10px;"/>
                                <br>

                                <div class="row">
                                <!-- Add Svg Icon-->
                                <p style="color: green"><svg 
                                    @click="addField(input, bills)"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 30 30"
                                    width="30"
                                    height="30"
                                    class="ml-2 cursor-pointer"
                                >
                                    <path fill="none" d="M0 0h24v24H0z" />
                                    <path fill="green" d="M11 11V7h2v4h4v2h-4v4h-2v-4H7v-2h4zm1 11C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16z"/>
                                </svg>เพิ่มใบเสร็จ</p>

                                <!-- Remove Svg Icon-->
                                <svg
                                    v-show="bills.length > 1"
                                    @click="removeField(index, bills)"
                                    xmlns="http://www.w3.org/2000/svg"
                                    viewBox="0 0 30 30"
                                    width="30"
                                    height="30"
                                    class="ml-2 cursor-pointer"
                                    style="margin-top: -43px; width: 80%;"
                                >
                                    <path fill="none" d="M0 0h24v24H0z"/>
                                    <path fill="#EC4899" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z"/>
                                </svg>
                                </div>
                            </div>
                        </form>
                    </div>
                    <br><br>
                    <p style="text-align: right;">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
                    <a class="btn-next" href="\Bookbank">{{ msg }}</a>
                </div>
            </div>
        </div>
        <br>
    </dev>
</template>

<script>
//Your Javascript lives within the Script Tag
export default {
  name: "AddRemove",
  data() {
    return {
        bills: [{ billNo: "",  hospital: "", bill_no: "", money: "", hospitalized_date: "", choose_file: ""}],
        phoneNumbers: [{ phone: "" }],
        image: '',
        total_amount: 4200, 
        msg: 'ดำเนินการต่อ'
    };
  },
  methods: {
    addField(value, fieldType) {
      fieldType.push({ value: "" });
    },
    removeField(index, fieldType) {
      //type.splice(index, 1);
      fieldType.splice(index, 1);
    },
    onFileChange(e) {
      var files = e.target.files || e.dataTransfer.files;
      if (!files.length)
        return;
      this.createImage(files[0]);
    },
    createImage(file) {
      var Image = new Image();
      var reader = new FileReader();
      var vm = this;

      reader.onload = (e) => {
        vm.image = e.target.result;
      };
      reader.readAsDataURL(file);
    },
    removeImage: function () {
      this.image = '';
    }
  },
};
</script>

<style>

input[type=text] {
  width: 100%;
  padding: 5px 10px;
  margin: 0px 0px;
  box-sizing: border-box;
  border: 2px solid #bbbbbb;
  border-radius: 4px;
  font-family: Taviraj;
  font-size: 13px;
}
input[type=file] {
  color: #5c2e91;
  width: 100%;
  padding: 2px 0px;
  margin: 0px 0px;
  border-radius: 4px;
  font-family: Taviraj;
  font-size: 13px;
}
label{
    font-family: Taviraj;
    font-size: 13px;
}

.stepper-wrapper {
  margin-top: auto;
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
}
.stepper-item {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  flex: 1;

  @media (max-width: 768px) {
    font-size: 12px;
  }
}

.stepper-item::before {
  position: absolute;
  content: "";
  border-bottom: 2px solid #bbb;
  width: 100%;
  top: 18px;
  left: -50%;
  z-index: 2;
}

.stepper-item::after {
  position: absolute;
  content: "";
  border-bottom: 2px solid #bbb;
  width: 100%;
  top: 18px;
  left: 50%;
  z-index: 2;
}

.stepper-item .step-counter {
  position: relative;
  z-index: 5;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 35px;
  height: 35px;
  border-radius: 50%;
  background: #bbb;
  margin-bottom: 2px;
}

.stepper-item.active {
  font-weight: bold;
}

.stepper-item.completed .step-counter {
  background-color: #5c2e91;
}

.stepper-item.completed::after {
  position: absolute;
  content: "";
  border-bottom: 2px solid #5c2e91;
  width: 100%;
  top: 18px;
  left: 50%;
  z-index: 3;
}

.stepper-item:first-child::before {
  content: none;
}
.stepper-item:last-child::after {
  content: none;
}
</style>
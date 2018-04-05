#pragma once
#include "Header.h"

namespace CppCLR_WinformsProjekt {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Zusammenfassung für Form1
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();
		
		}

	protected:
		/// <summary>
	
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Button^  button1;
	private: System::Windows::Forms::Button^  button2;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Button^  button3;
	private: System::Windows::Forms::PictureBox^  pictureBox1;

	protected:

	private:
		/// <summary>
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// </summary>
		void InitializeComponent(void)
		{
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->button2 = (gcnew System::Windows::Forms::Button());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->button3 = (gcnew System::Windows::Forms::Button());
			this->pictureBox1 = (gcnew System::Windows::Forms::PictureBox());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pictureBox1))->BeginInit();
			this->SuspendLayout();
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(452, 42);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(43, 13);
			this->label1->TabIndex = 0;
			this->label1->Text = L"Camera";
			this->label1->Click += gcnew System::EventHandler(this, &Form1::label1_Click);
			// 
			// button1
			// 
			this->button1->Location = System::Drawing::Point(417, 64);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(110, 35);
			this->button1->TabIndex = 1;
			this->button1->Text = L"On";
			this->button1->UseVisualStyleBackColor = true;
			this->button1->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// button2
			// 
			this->button2->Location = System::Drawing::Point(417, 117);
			this->button2->Name = L"button2";
			this->button2->Size = System::Drawing::Size(110, 35);
			this->button2->TabIndex = 2;
			this->button2->Text = L"Off";
			this->button2->UseVisualStyleBackColor = true;
			this->button2->Click += gcnew System::EventHandler(this, &Form1::button2_Click);
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(429, 176);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(91, 13);
			this->label2->TabIndex = 3;
			this->label2->Text = L"Face Recognition";
			// 
			// button3
			// 
			this->button3->Location = System::Drawing::Point(417, 202);
			this->button3->Name = L"button3";
			this->button3->Size = System::Drawing::Size(110, 35);
			this->button3->TabIndex = 4;
			this->button3->Text = L"On";
			this->button3->UseVisualStyleBackColor = true;
			this->button3->Click += gcnew System::EventHandler(this, &Form1::button3_Click);
			// 
			// pictureBox1
			// 
			this->pictureBox1->Location = System::Drawing::Point(12, 12);
			this->pictureBox1->Name = L"pictureBox1";
			this->pictureBox1->Size = System::Drawing::Size(399, 307);
			this->pictureBox1->TabIndex = 5;
			this->pictureBox1->TabStop = false;
			this->pictureBox1->Click += gcnew System::EventHandler(this, &Form1::pictureBox1_Click_1);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(548, 344);
			this->Controls->Add(this->pictureBox1);
			this->Controls->Add(this->button3);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->button1);
			this->Controls->Add(this->label1);
			this->Name = L"Form1";
			this->Text = L"Form1";
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->pictureBox1))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
		System::Void DrawCvImage(const cv::Mat& cvImage)
		{
			assert(cvImage.type() == CV_8UC3);

			if ((pictureBox1->Image == nullptr) || (pictureBox->Width != cvImage.cols) || (pictureBox->Height != cvImage.rows))
			{
				pictureBox1->Width = cvImage.cols;
				pictureBox1->Height = cvImage.rows;
				pictureBox1->Image = gcnew System::Drawing::Bitmap(cvImage.cols, cvImage.rows);
			}

			System::Drawing::Bitmap^ bmpImage = gcnew Bitmap(
				cvImage.cols, cvImage.rows, cvImage.step,
				System::Drawing::Imaging::PixelFormat::Format24bppRgb,
				System::IntPtr(cvImage.data)
			);

			Graphics^ g = Graphics::FromImage(pictureBox->Image);

			g->DrawImage(bmpImage, 0, 0, cvImage.cols, cvImage.rows);
			pictureBox->Refresh();

			delete g;
		}




#pragma endregion
	private: System::Void label1_Click(System::Object^  sender, System::EventArgs^  e) {
	}

	private: System::Void pictureBox1_Click(System::Object^  sender, System::EventArgs^  e) {
	}





private: System::Void button3_Click(System::Object^  sender, System::EventArgs^  e) {
	while (true) {
		Mat cameraFrame;
		stream1.read(cameraFrame);
		cameraFrame=FaceDetection(cameraFrame);
		DrawCvImage(cameraFrame);
		if (waitKey(30) >= 0)
			break;
}

private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
	VideoCapture stream1(0);  

	while (true) {
		Mat cameraFrame;
		stream1.read(cameraFrame);
		DrawCvImage(cameraFrame);
		if (waitKey(30) >= 0)
			break;
}
private: System::Void button2_Click(System::Object^  sender, System::EventArgs^  e) {
}
};